using NoteMapper.Core.Instruments;
using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapCriteriaOptionsViewModel
    {
        public NoteMapCriteriaOptionsViewModel(IEnumerable<InstrumentBase> defaultInstruments, 
            IEnumerable<InstrumentBase> userInstruments,
            IReadOnlyCollection<string> keyNames,
            IReadOnlyCollection<string> keyTypes)
        {
            DefaultInstruments = defaultInstruments.ToArray();
            UserInstruments = userInstruments.ToArray();

            KeyNames = keyNames;
            KeyTypes = keyTypes;

            Types = new[]
            {
                NoteMapType.Chord
            };
        }

        public IReadOnlyCollection<InstrumentBase> DefaultInstruments { get; }

        public IReadOnlyCollection<string> KeyNames { get; }

        public IReadOnlyCollection<string> KeyTypes { get; }

        public IReadOnlyCollection<NoteMapType> Types { get; }

        public IReadOnlyCollection<InstrumentBase> UserInstruments { get; }
    }
}
