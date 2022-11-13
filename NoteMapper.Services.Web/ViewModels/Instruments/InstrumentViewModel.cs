using NoteMapper.Core.Instruments;

namespace NoteMapper.Services.Web.ViewModels.Instruments
{
    public class InstrumentViewModel
    {
        public InstrumentViewModel(StringedInstrumentBase instrument)
        {
            Strings = instrument
                .Strings
                .Select(x => new InstrumentStringViewModel(x))
                .ToArray();
        }

        public IReadOnlyCollection<InstrumentStringViewModel> Strings { get; }
    }
}
