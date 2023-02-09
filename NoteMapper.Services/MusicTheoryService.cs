using NoteMapper.Core.Extensions;
using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Services
{
    public class MusicTheoryService : IMusicTheoryService
    {
        private static readonly IReadOnlyCollection<string> _keyTypes = Enum.GetValues<ScaleType>()
            .Select(x => x.ShortName())
            .ToArray();

        public IReadOnlyCollection<string> GetKeyNames(AccidentalType accidental)
        {
            return Note.GetNotes(accidental);
        }

        public IReadOnlyCollection<string> GetScaleTypes()
        {
            return _keyTypes;
        }
    }
}
