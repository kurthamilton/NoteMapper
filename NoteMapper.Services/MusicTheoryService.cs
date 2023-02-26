using NoteMapper.Core.Extensions;
using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Services
{
    public class MusicTheoryService : IMusicTheoryService
    {
        private static readonly IReadOnlyCollection<ScaleType> _keyTypes = Enum.GetValues<ScaleType>()
            .ToArray();

        public IReadOnlyCollection<int> GetNoteIndexes()
        {
            return Note.GetNoteIndexes();
        }

        public IReadOnlyCollection<ScaleType> GetScaleTypes()
        {
            return _keyTypes;
        }
    }
}
