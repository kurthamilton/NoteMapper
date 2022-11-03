using NoteMapper.Core.Instruments;
using NoteMapper.Core.Instruments.Implementations;

namespace NoteMapper.Services
{
    public class InstrumentFactory : IInstrumentFactory
    {
        public InstrumentBase? GetInstrument(string? name)
        {
            return GetInstruments()
                .FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyCollection<InstrumentBase> GetInstruments()
        {
            return new InstrumentBase[]
            {
                PedalSteelGuitar.C6(),
                PedalSteelGuitar.E9()
            };
        }
    }
}
