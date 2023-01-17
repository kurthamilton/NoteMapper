﻿using NoteMapper.Core;

namespace NoteMapper.Data.Core.Instruments
{
    public interface IUserInstrumentRepository
    {
        Task<ServiceResult> CreateUserInstrumentAsync(Guid userId, UserInstrument instrument);

        Task<ServiceResult> DeleteUserInstrumentAsync(Guid userId, string userInstrumentId);

        Task<UserInstrument?> FindAsync(string userInstrumentId);

        Task<UserInstrument?> FindAsync(Guid userId, string userInstrumentId);

        Task<IReadOnlyCollection<UserInstrument>> GetDefaultInstrumentsAsync();

        Task<IReadOnlyCollection<UserInstrument>> GetUserInstrumentsAsync(Guid userId);

        Task<ServiceResult> UpdateUserInstrumentAsync(Guid userId, UserInstrument instrument);
    }
}