﻿using Microsoft.AspNetCore.Identity;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Identity.Microsoft
{
    public static class UserMapper
    {
        public static IdentityUser ToIdentityUser(this User user)
        {
            IdentityUser identityUser = new IdentityUser(user.Email)
            {            
                Email = user.Email,
                Id = user.UserId.ToString(),                
                UserName = user.Email                
            };            
            
            return identityUser;
        }
    }
}