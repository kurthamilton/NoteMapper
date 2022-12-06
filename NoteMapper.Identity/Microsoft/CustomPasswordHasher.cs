using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace NoteMapper.Identity.Microsoft
{
    public class CustomPasswordHasher : IPasswordHasher<IdentityUser>, IPasswordHasher
    {
        private readonly CustomPasswordHasherSettings _settings;

        public CustomPasswordHasher(CustomPasswordHasherSettings settings)
        {
            _settings = settings;
        }

        public string EncodeSalt(byte[] salt)
        {
            return Convert.ToBase64String(salt);
        }

        public byte[] GenerateSalt()
        {
            using (RandomNumberGenerator saltGenerator = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[_settings.SaltByteSize];
                saltGenerator.GetBytes(salt);
                return salt;
            }
        }

        public string HashPassword(string plainText, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            return HashPassword(plainText, saltBytes);
        }

        public string HashPassword(string plainText, byte[] salt)
        {
            byte[] hashBytes = ComputeHash(plainText, salt);

            string hash = Convert.ToBase64String(hashBytes);
            return hash;
        }

        public string HashPassword(IdentityUser user, string password)
        {
            return password;
        }

        public PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword, string providedPassword)
        {
            return hashedPassword == providedPassword
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed;
        }

        private byte[] ComputeHash(string plainText, byte[] salt)
        {
            using (DeriveBytes hashGenerator = GetHashGenerator(plainText, salt))
            {
                return hashGenerator.GetBytes(_settings.HashByteSize);
            }
        }

        private DeriveBytes GetHashGenerator(string plainText, byte[] salt)
        {
            return new Rfc2898DeriveBytes(plainText, salt, _settings.HashIterations, HashAlgorithmName.SHA256);
        }
    }
}