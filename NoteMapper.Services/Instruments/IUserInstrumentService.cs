using NoteMapper.Core;
using NoteMapper.Core.Instruments;
using NoteMapper.Data.Core.Instruments;

namespace NoteMapper.Services.Instruments
{
    public interface IUserInstrumentService
    {
        Task<ServiceResult> CreateInstrumentAsync(Guid userId, UserInstrument instrument);

        Task<ServiceResult> DeleteInstrumentAsync(Guid userId, string userInstrumentId);

        Task<InstrumentBase?> FindAsync(Guid userId, string userInstrumentId);

        Task<UserInstrument?> FindUserInstrumentAsync(Guid userId, string userInstrumentId);

        Task<IReadOnlyCollection<InstrumentBase>> GetDefaultInstrumentsAsync();

        Task<IReadOnlyCollection<InstrumentBase>> GetUserInstrumentsAsync(Guid userId);

        Task<ServiceResult> UpdateInstrumentAsync(Guid userId, UserInstrument instrument);
    }
}
