﻿using NoteMapper.Core;

namespace NoteMapper.Data.Core.Users
{
    public interface IUserRepository
    {
        Task<ServiceResult> CreateAsync(User user);  
        
        Task<ServiceResult> DeleteAsync(Guid userId);

        Task<User?> FindAsync(Guid userId);

        Task<User?> FindByEmailAsync(string email);
    }
}