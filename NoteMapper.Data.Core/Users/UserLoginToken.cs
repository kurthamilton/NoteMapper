﻿namespace NoteMapper.Data.Core.Users
{
    public class UserLoginToken
    {
        public UserLoginToken(Guid userId, DateTime createdUtc, DateTime expiresUtc, string token)
        {
            CreatedUtc = createdUtc;
            ExpiresUtc = expiresUtc;
            Token = token;
            UserId = userId;
        }

        public DateTime CreatedUtc { get; }

        public DateTime ExpiresUtc { get; }

        public string Token { get; }

        public Guid UserId { get; }
    }
}
