namespace NoteMapper.Services
{
    public interface IMusicTheoryService
    {        
        IReadOnlyCollection<int> GetNoteIndexes();

        IReadOnlyCollection<string> GetScaleTypes();
    }
}
