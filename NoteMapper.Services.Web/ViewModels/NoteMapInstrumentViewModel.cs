using NoteMapper.Core.Instruments;

namespace NoteMapper.Services.Web.ViewModels
{
    public class NoteMapInstrumentViewModel
    {
        public NoteMapInstrumentViewModel(StringedInstrumentBase instrument)
        {
            Strings = instrument
                .Strings
                .Select(x => new NoteMapStringViewModel(x))
                .ToArray();
        }

        public IReadOnlyCollection<NoteMapStringViewModel> Strings { get; }
    }
}
