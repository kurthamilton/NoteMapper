using NoteMapper.Core;

namespace NoteMapper.Data.Core.Instruments
{
    public interface IUserInstrumentRepository
    {
        Task<ServiceResult> CreateUserInstrumentAsync(Guid userId, UserInstrument instrument);

        Task<ServiceResult> DeleteUserInstrumentAsync(Guid userId, UserInstrument instrument);

        Task<IReadOnlyCollection<UserInstrument>> GetDefaultInstrumentsAsync();

        Task<IReadOnlyCollection<UserInstrument>> GetUserInstrumentsAsync(Guid userId);

        Task<ServiceResult> UpdateUserInstrumentAsync(Guid userId, UserInstrument instrument);
    }
}
