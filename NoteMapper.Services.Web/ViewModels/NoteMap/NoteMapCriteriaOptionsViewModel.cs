using NoteMapper.Core.Guitars;
using NoteMapper.Core.MusicTheory;

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

            Types = new[]
            {
                NoteMapType.Chord
            };
        }

        public IReadOnlyCollection<GuitarBase> DefaultInstruments { get; }

        public IReadOnlyCollection<string> KeyNames { get; }

        public IReadOnlyCollection<string> ScaleTypes { get; }

        public IReadOnlyCollection<NoteMapType> Types { get; }

        public IReadOnlyCollection<GuitarBase> UserInstruments { get; }
    }
}
