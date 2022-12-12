﻿using NoteMapper.Core;
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
        private readonly IUserPasswordResetCodeRepository _userPasswordResetCodeRepository;
        private readonly IUserRegistrationCodeRepository _userRegistrationCodeRepository;
        private readonly IUserRepository _userRepository;

        public MicrosoftIdentityService(IUserRepository userRepository, IUserActivationRepository userActivationRepository,
            MicrosoftIdentityServiceSettings settings, IPasswordHasher passwordHasher, IUserPasswordRepository userPasswordRepository,
            IUserLoginTokenRepository userLoginTokenRepository, IEmailSenderService emailSenderService,
            IUrlEncoder urlEncoder, IRegistrationCodeRepository registrationCodeRepository, 
            IUserPasswordResetCodeRepository userPasswordResetCodeRepository, 
            IUserRegistrationCodeRepository userRegistrationCodeRepository)
        {
            _emailSenderService = emailSenderService;
            _passwordHasher = passwordHasher;
            _registrationCodeRepository = registrationCodeRepository;
            _settings = settings;
            _urlEncoder = urlEncoder;
            _userActivationRepository = userActivationRepository;
            _userLoginTokenRepository = userLoginTokenRepository;
            _userPasswordRepository = userPasswordRepository;
            _userRegistrationCodeRepository = userRegistrationCodeRepository;
            _userRepository = userRepository;
            _userPasswordResetCodeRepository = userPasswordResetCodeRepository;
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

        public Task<UserLoginToken?> CreateLoginTokenAsync(Guid userId)
        {
            DateTime createdUtc = DateTime.UtcNow;
            DateTime expiresUtc = createdUtc.AddSeconds(_settings.LoginTokenExpiresAfterSeconds);
            string token = Guid.NewGuid().ToString();

            UserLoginToken loginToken = new UserLoginToken(userId, createdUtc, expiresUtc, token);

            return _userLoginTokenRepository.CreateAsync(loginToken);
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

            RegistrationCode? registrationCode = null;
            if (registrationType == RegistrationType.InviteOnly)
            {
                registrationCode = !string.IsNullOrEmpty(code) 
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

            User? user = new User(Guid.Empty, createdUtc, email);            
            user = await _userRepository.CreateAsync(user);
            if (user == null)
            {
                return defaultErrorResult;
            }

            string activationCode = Guid.NewGuid().ToString();
            DateTime activationCodeExpiresUtc = createdUtc.AddMinutes(_settings.ActivationCodeExpiresAfterMinutes);

            UserActivation? userActivation = new UserActivation(user.UserId, createdUtc, activationCodeExpiresUtc, activationCode);
            userActivation = await _userActivationRepository.CreateAsync(userActivation);
            if (userActivation == null)
            {
                return defaultErrorResult;
            }

            if (registrationCode != null)
            {
                UserRegistrationCode? userRegistrationCode = new UserRegistrationCode(user.UserId, 
                    registrationCode.RegistrationCodeId, createdUtc);
                await _userRegistrationCodeRepository.CreateAsync(userRegistrationCode);
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

            IReadOnlyCollection<UserActivation> activations = await _userActivationRepository.GetAllAsync(user.UserId);
            if (activations.Count > 0)
            {
                // Do not allow password reset if the user has a pending activation
                // Activations are cleared out after the user is activated.
                return defaultResult;
            }

            DateTime createdUtc = DateTime.UtcNow;
            string code = Guid.NewGuid().ToString();
            DateTime expiresUtc = createdUtc.AddHours(_settings.PasswordResetCodeExpiresAfterHours);
            UserPasswordResetCode userPasswordResetCode = new(user.UserId, createdUtc, expiresUtc, code);            

            UserPasswordResetCode? createResult = await _userPasswordResetCodeRepository.CreateAsync(userPasswordResetCode);
            if (createResult == null)
            {
                return defaultErrorResult;
            }

            ServiceResult emailResult = await SendPasswordResetEmailAsync(user, createResult);
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

            UserPasswordResetCode? userPasswordResetCode = await _userPasswordResetCodeRepository.FindAsync(user.UserId, code);            
            if (userPasswordResetCode == null ||
                userPasswordResetCode.Code != code ||
                userPasswordResetCode.ExpiresUtc < DateTime.UtcNow)
            {
                return defaultResult;
            }

            ServiceResult passwordResult = await SetUserPasswordAsync(user.UserId, newPassword);
            if (passwordResult.Success)
            {
                await _userPasswordResetCodeRepository.DeleteAllAsync(user.UserId);
            }

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

            userPassword = new(userId, newHash, newSalt);
            
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

        private Task<ServiceResult> SendPasswordResetEmailAsync(User user, UserPasswordResetCode userPasswordResetCode)
        {
            string url = _settings.PasswordResetUrl
                .Replace("{code}", _urlEncoder.UrlEncode(userPasswordResetCode.Code))
                .Replace("{email}", _urlEncoder.UrlEncode(user.Email));

            string body =
                "<p>A Note Mapper password reset request has been made for this email address.</p>" +
                "<p>Please click on the link below to reset your password</p>" +
                @$"<p><a href=""{url}"">{url}</a></p>";
            Email email = new(user.Email, "Reset your Note Mapper password", body);
            return _emailSenderService.SendEmailAsync(email);
        }

        private Task<ServiceResult> SetUserPasswordAsync(Guid userId, string plainText)
        {
            string salt = _passwordHasher.GenerateSalt();
            string hash = _passwordHasher.HashPassword(plainText, salt);

            UserPassword userPassword = new(userId, hash, salt);
            return _userPasswordRepository.UpdateAsync(userPassword);
        }
    }
}
