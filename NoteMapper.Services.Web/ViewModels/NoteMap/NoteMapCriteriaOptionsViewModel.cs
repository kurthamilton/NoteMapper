using NoteMapper.Core.Guitars;
using NoteMapper.Core.MusicTheory;
using NoteMapper.Core.NoteMap;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapCriteriaOptionsViewModel
    {
        public NoteMapCriteriaOptionsViewModel(IEnumerable<GuitarBase> defaultInstruments, 
            IEnumerable<GuitarBase> userInstruments,
            IReadOnlyCollection<int> noteIndexes,
            IReadOnlyCollection<string> scaleTypes)
        {
            DefaultInstruments = defaultInstruments.ToArray();
            UserInstruments = userInstruments.ToArray();

            NoteIndexes = noteIndexes;
            ScaleTypes = scaleTypes;

            Accidentals = new[]
            {
                Accidental.ToString(AccidentalType.Sharp),
                Accidental.ToString(AccidentalType.Flat)
            };

            Modes = new[]
            {
                NoteMapMode.Combinations,
                NoteMapMode.Manual
            };

            Types = new[]
            {
                NoteCollectionType.Chord,
                NoteCollectionType.Scale
            };
        }

        public IReadOnlyCollection<string> Accidentals { get; }

        public IReadOnlyCollection<GuitarBase> DefaultInstruments { get; }

        public IReadOnlyCollection<NoteMapMode> Modes { get; }

        public IReadOnlyCollection<int> NoteIndexes { get; }

        public IReadOnlyCollection<string> ScaleTypes { get; }

        public IReadOnlyCollection<NoteCollectionType> Types { get; }

        public IReadOnlyCollection<GuitarBase> UserInstruments { get; }
    }
}
