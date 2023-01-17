using NoteMapper.Core.Guitars;

namespace NoteMapper.Services.Web.ViewModels.Instruments
{
    public class InstrumentViewModel
    {
        public InstrumentViewModel(GuitarBase instrument)
        {
            Strings = instrument
                .Strings
                .Select(x => new InstrumentStringViewModel(x))
                .ToArray();
        }

        public IReadOnlyCollection<InstrumentStringViewModel> Strings { get; }
    }
}
