using NoteMapper.Core.Instruments;

namespace NoteMapper.Services.Instruments
{
    public interface IUserInstrumentService
    {
        Task<IReadOnlyCollection<InstrumentBase>> GetDefaultInstrumentsAsync();

        Task<IReadOnlyCollection<InstrumentBase>> GetUserInstrumentsAsync(Guid userId);
    }
}
