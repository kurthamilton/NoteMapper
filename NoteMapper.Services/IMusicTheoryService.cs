using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Services
{
    public interface IMusicTheoryService
    {        
        IReadOnlyCollection<int> GetNoteIndexes();

        IReadOnlyCollection<ScaleType> GetScaleTypes();
    }
}
