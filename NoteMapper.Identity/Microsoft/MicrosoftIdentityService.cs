using NoteMapper.Core;
using NoteMapper.Core.Users;
using NoteMapper.Data.Core.Users;
using NoteMapper.Services.Emails;
using NoteMapper.Services.Users;
using NoteMapper.Services.Web;

namespace NoteMapper.Identity.Microsoft
{
    public class MicrosoftIdentityService : IIdentityService
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRegistrationCodeRepository _registrationCodeRepository;
        private readonly MicrosoftIdentityServiceSettings _settings;
        private readonly IUrlEncoder _urlEncoder;
        private readonly IUserActivationRepository _userActivationRepository;
        private readonly IUserLoginTokenRepository _userLoginTokenRepository;        
        private readonly IUserPasswordRepository _userPasswordRepository;
        private readonly IUserRepository _userRepository;

        public MicrosoftIdentityService(IUserRepository userRepository, IUserActivationRepository userActivationRepository,
            MicrosoftIdentityServiceSettings settings, IPasswordHasher passwordHasher, IUserPasswordRepository userPasswordRepository,
            IUserLoginTokenRepository userLoginTokenRepository, IEmailSenderService emailSenderService,
            IUrlEncoder urlEncoder, IRegistrationCodeRepository registrationCodeRepository)
        {
            _emailSenderService = emailSenderService;
            _passwordHasher = passwordHasher;
            _registrationCodeRepository = registrationCodeRepository;
            _settings = settings;
            _urlEncoder = urlEncoder;
            _userActivationRepository = userActivationRepository;
            _userLoginTokenRepository = userLoginTokenRepository;
            _userPasswordRepository = userPasswordRepository;
            _userRepository = userRepository;
        }

        public async Task<ServiceResult> ActivateUserAsync(string email, string code, string password)
        {
            ServiceResult defaultResult = ServiceResult.Failure("Invalid account activation link");

            User? user = await FindUserAsync(email);
            if (user == null)
            {
                return defaultResult;
            }

            UserActivation? activation = await _userActivationRepository.FindAsync(user.UserId, code);
            if (activation == null)
            {
                return defaultResult;
            }

            if (activation.ExpiresUtc < DateTime.UtcNow)
            {
                return defaultResult;
            }

            ServiceResult passwordResult = await SetUserPasswordAsync(user.UserId, password);
            if (!passwordResult.Success)
            {
                return ServiceResult.Failure("An error occurred when updating your password");
            }

            await _userActivationRepository.DeleteAllAsync(user.UserId);

            return ServiceResult.Successful("Your account has been activated");
        }

        public async Task<ServiceResult> CanUserSignIn(Guid userId, string password)
        {
            ServiceResult defaultResult = ServiceResult.Failure("Invalid email or password");

            UserPassword? userPassword = await FindUserPasswordAsync(userId);
            if (userPassword == null)
            {
                return defaultResult;
            }

            string hashedPassword = _passwordHasher.HashPassword(password, userPassword.Salt);
            if (hashedPassword != userPassword.Hash)
            {
                return defaultResult;
            }

            return ServiceResult.Successful();
        }

        public async Task<UserLoginToken?> CreateLoginTokenAsync(Guid userId)
        {
            DateTime createdUtc = DateTime.UtcNow;
            DateTime expiresUtc = createdUtc.AddSeconds(_settings.LoginTokenExpiresAfterSeconds);
            string token = Guid.NewGuid().ToString();

            UserLoginToken loginToken = new UserLoginToken
            {
                CreatedUtc = createdUtc,
                ExpiresUtc = expiresUtc,
                Token = token,
                UserId = userId
            };
            
            ServiceResult result = await _userLoginTokenRepository.CreateAsync(loginToken);
            return result.Success
                ? loginToken
                : null;
        }

        public async Task<ServiceResult> DeleteAccountAsync(Guid userId)
        {
            ServiceResult result = await _userRepository.DeleteAsync(userId);
            return result.Success
                ? ServiceResult.Successful("Your account has been deleted")
                : ServiceResult.Failure("An error occurred while deleting your account");
        }

        public Task<User?> FindUserAsync(Guid userId)
        {
            return _userRepository.FindAsync(userId);
        }

        public Task<User?> FindUserAsync(string email)
        {
            return _userRepository.FindByEmailAsync(email);
        }

        public RegistrationType GetRegistrationType()
        {
            return _settings.RegistrationType;
        }

        public async Task<ServiceResult> RegisterUserAsync(string email, string? code)
        {
            RegistrationType registrationType = GetRegistrationType();
            if (registrationType == RegistrationType.Closed)
            {
                return ServiceResult.Failure("Registration is currently closed");
            }

            if (registrationType == RegistrationType.InviteOnly)
            {
                RegistrationCode? registrationCode = !string.IsNullOrEmpty(code) 
                    ? await _registrationCodeRepository.FindAsync(code)
                    : null;

                if (registrationCode == null || registrationCode.ExpiresUtc < DateTime.UtcNow)
                {
                    return ServiceResult.Failure("Invalid registration code");
                }
            }

            ServiceResult defaultResult = ServiceResult.Successful($"An activation email has been sent to {email}. " +
                "If you have already registered then login or reset your password.");
            ServiceResult defaultErrorResult = ServiceResult.Failure("An error has occurred while creating your account");

            User? existing = await FindUserAsync(email);
            if (existing != null)
            {
                return defaultResult;
            }

            DateTime createdUtc = DateTime.UtcNow;                        

            User user = new User
            {
                CreatedUtc = createdUtc,
                Email = email,
            };
            
            ServiceResult userResult = await _userRepository.CreateAsync(user);
            if (!userResult.Success)
            {
                return defaultErrorResult;
            }

            string activationCode = Guid.NewGuid().ToString();
            DateTime activationCodeExpiresUtc = createdUtc.AddMinutes(_settings.ActivationCodeExpiresAfterMinutes);

            UserActivation userActivation = new UserActivation
            {
                Code = activationCode,
                CreatedUtc = createdUtc,
                ExpiresUtc = activationCodeExpiresUtc,
                UserId = user.UserId
            };
            ServiceResult activationResult = await _userActivationRepository.CreateAsync(userActivation);
            if (!activationResult.Success)
            {
                return defaultErrorResult;
            }

            ServiceResult activationEmailResult = await SendActivationEmailAsync(user, userActivation);
            if (!activationEmailResult.Success)
            {
                return defaultErrorResult;
            }

            return defaultResult;
        }

        public async Task<ServiceResult> RequestPasswordResetAsync(string email)
        {
            ServiceResult defaultResult = ServiceResult.Successful($"A password reset link has been sent to {email} if it is registered");
            ServiceResult defaultErrorResult = ServiceResult.Failure("An error occurred while resetting your password");

            User? user = await FindUserAsync(email);
            if (user == null)
            {
                return defaultResult;
            }

            UserPassword? userPassword = await _userPasswordRepository.FindAsync(user.UserId);
            if (userPassword == null)
            {
                return defaultResult;
            }

            string resetCode = Guid.NewGuid().ToString();
            DateTime resetCodeExpiresUtc = DateTime.UtcNow.AddHours(_settings.PasswordResetCodeExpiresAfterHours);
            userPassword.SetResetCode(resetCode, resetCodeExpiresUtc);

            ServiceResult updateResult = await _userPasswordRepository.UpdateAsync(userPassword);
            if (!updateResult.Success)
            {
                return defaultErrorResult;
            }

            ServiceResult emailResult = await SendPasswordResetEmailAsync(user, userPassword);
            if (!emailResult.Success)
            {
                return defaultErrorResult;
            }

            return defaultResult;
        }

        public async Task<ServiceResult> ResetPasswordAsync(string email, string code, string newPassword)
        {
            ServiceResult defaultResult = ServiceResult.Failure("The link you followed is invalid or has expired");

            User? user = await FindUserAsync(email);
            if (user == null)
            {
                return defaultResult;
            }

            UserPassword? password = await _userPasswordRepository.FindAsync(user.UserId);
            if (password == null || 
                string.IsNullOrEmpty(password.ResetCode) || 
                password.ResetCode != code || 
                password.ResetCodeExpiresUtc < DateTime.UtcNow)
            {
                return defaultResult;
            }

            ServiceResult passwordResult = await SetUserPasswordAsync(password, newPassword);
            return passwordResult.Success
                ? ServiceResult.Successful("Your password has been reset")
                : defaultResult;
        }

        public async Task<ServiceResult> UpdatePasswordAsync(Guid userId, string oldPassword, string newPassword)
        {
            ServiceResult defaultResult = ServiceResult.Failure("An error while updating your password");

            UserPassword? userPassword = await FindUserPasswordAsync(userId);
            if (userPassword == null)
            {
                return defaultResult;
            }

            string hashedOldPassword = _passwordHasher.HashPassword(oldPassword, userPassword.Salt);
            if (hashedOldPassword != userPassword.Hash)
            {
                return ServiceResult.Failure("Old password mismatch");
            }

            string newSalt = _passwordHasher.GenerateSalt();
            string newHash = _passwordHasher.HashPassword(newPassword, newSalt);

            userPassword.Hash = newHash;
            userPassword.Salt = newSalt;

            ServiceResult result = await _userPasswordRepository.UpdateAsync(userPassword);
            return result.Success
                ? ServiceResult.Successful("Password updated")
                : defaultResult;
        }

        public async Task<User?> UseLoginTokenAsync(string email, string token)
        {
            User? user = await FindUserAsync(email);
            if (user == null)
            {
                return null;
            }

            UserLoginToken? loginToken = await _userLoginTokenRepository.FindAsync(user.UserId, token);
            if (loginToken == null)
            {
                return null;
            }

            await _userLoginTokenRepository.DeleteAllAsync(user.UserId);

            return loginToken.ExpiresUtc < DateTime.UtcNow
                ? null
                : user;
        }

        private async Task<UserPassword?> FindUserPasswordAsync(Guid userId)
        {
            User? user = await FindUserAsync(userId);
            if (user == null)
            {
                return null;
            }

            UserPassword? userPassword = await _userPasswordRepository.FindAsync(user.UserId);
            return userPassword;
        }

        private Task<ServiceResult> SendActivationEmailAsync(User user, UserActivation activation)
        {
            string url = _settings.ActivationUrl
                .Replace("{email}", _urlEncoder.UrlEncode(user.Email))
                .Replace("{code}", _urlEncoder.UrlEncode(activation.Code));

            string body = 
                "<p>Welcome to Note Mapper</p>" +
                "<p>Please use the link below to activate your account.</p>" +
                @$"<p><a href=""{url}"">{url}</a></p>";
            Email email = new Email(user.Email, "Activate your Note Mapper account", body);
            return _emailSenderService.SendEmailAsync(email);
        }

        private Task<ServiceResult> SendPasswordResetEmailAsync(User user, UserPassword password)
        {
            string url = _settings.PasswordResetUrl
                .Replace("{code}", _urlEncoder.UrlEncode(password.ResetCode ?? ""))
                .Replace("{email}", _urlEncoder.UrlEncode(user.Email));

            string body =
                "<p>A Note Mapper password reset request has been made for this email address.</p>" +
                "<p>Please click on the link below to reset your password</p>" +
                @$"<p><a href=""{url}"">{url}</a></p>";
            Email email = new(user.Email, "Reset your Note Mapper password", body);
            return _emailSenderService.SendEmailAsync(email);
        }

        private async Task<ServiceResult> SetUserPasswordAsync(Guid userId, string plainText)
        {
            UserPassword? userPassword = await _userPasswordRepository.FindAsync(userId);
            if (userPassword == null)
            {
                userPassword = new()
                {
                    UserId = userId
                };
            }

            return await SetUserPasswordAsync(userPassword, plainText);
        }

        private async Task<ServiceResult> SetUserPasswordAsync(UserPassword userPassword, string plainText)
        {
            string salt = _passwordHasher.GenerateSalt();
            string hash = _passwordHasher.HashPassword(plainText, salt);

            bool firstTime = userPassword.UserPasswordId == Guid.Empty;
            userPassword.Hash = hash;            
            userPassword.Salt = salt;
            userPassword.RemoveResetCode();

            return firstTime
                ? await _userPasswordRepository.CreateAsync(userPassword)
                : await _userPasswordRepository.UpdateAsync(userPassword);
        }
    }
}
