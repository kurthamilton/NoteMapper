using NoteMapper.Core;
using NoteMapper.Core.Instruments;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapCriteriaOptionsViewModel
    {
        public NoteMapCriteriaOptionsViewModel(IEnumerable<InstrumentBase> instruments, IReadOnlyCollection<string> keyNames,
            IReadOnlyCollection<string> keyTypes)
        {
            Instruments = instruments
                .Select(x => x.Name)
                .ToArray();

            KeyNames = keyNames;
            KeyTypes = keyTypes;

            Types = new[]
            {
                NoteMapType.Chord
            };
        }

        public IReadOnlyCollection<string> Instruments { get; }        

        public IReadOnlyCollection<string> KeyNames { get; }

        public IReadOnlyCollection<string> KeyTypes { get; }

        public IReadOnlyCollection<NoteMapType> Types { get; }
    }
}
