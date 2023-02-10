using NoteMapper.Core.Extensions;
using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Services
{
    public class MusicTheoryService : IMusicTheoryService
    {
        private static readonly IReadOnlyCollection<string> _keyTypes = Enum.GetValues<ScaleType>()
            .Select(x => x.ShortName())
            .ToArray();

        public IReadOnlyCollection<int> GetNoteIndexes()
        {
            return Note.GetNoteIndexes();
        }

        public IReadOnlyCollection<string> GetScaleTypes()
        {
            return _keyTypes;
        }
    }
}
