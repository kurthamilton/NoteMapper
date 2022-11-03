using NoteMapper.Core.Instruments;

namespace NoteMapper.Services
{
    public interface IInstrumentFactory
    {
        InstrumentBase? GetInstrument(string? name);

        IReadOnlyCollection<InstrumentBase> GetInstruments();
    }
}
