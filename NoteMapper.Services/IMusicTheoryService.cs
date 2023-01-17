using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Services
{
    public interface IMusicTheoryService
    {
        Key? GetKey(string shortName);

        IReadOnlyCollection<string> GetKeyNames();        

        IReadOnlyCollection<Key> GetKeys();

        IReadOnlyCollection<string> GetScaleTypes();
    }
}
