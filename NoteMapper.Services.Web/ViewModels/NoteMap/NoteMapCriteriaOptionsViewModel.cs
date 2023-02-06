using NoteMapper.Core.Guitars;
using NoteMapper.Core.MusicTheory;
using NoteMapper.Core.NoteMap;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapCriteriaOptionsViewModel
    {
        public NoteMapCriteriaOptionsViewModel(IEnumerable<GuitarBase> defaultInstruments, 
            IEnumerable<GuitarBase> userInstruments,
            IReadOnlyCollection<string> keyNames,
            IReadOnlyCollection<string> scaleTypes)
        {
            DefaultInstruments = defaultInstruments.ToArray();
            UserInstruments = userInstruments.ToArray();

            KeyNames = keyNames;
            ScaleTypes = scaleTypes;

            Modes = new[]
            {
                NoteMapMode.Permutations,
                NoteMapMode.Manual
            };

            Types = new[]
            {
                NoteMapType.Chord
            };
        }

        public IReadOnlyCollection<GuitarBase> DefaultInstruments { get; }

        public IReadOnlyCollection<string> KeyNames { get; }

        public IReadOnlyCollection<NoteMapMode> Modes { get; }

        public IReadOnlyCollection<string> ScaleTypes { get; }

        public IReadOnlyCollection<NoteMapType> Types { get; }

        public IReadOnlyCollection<GuitarBase> UserInstruments { get; }
    }
}
