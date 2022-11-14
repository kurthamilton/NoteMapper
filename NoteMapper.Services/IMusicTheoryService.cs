using NoteMapper.Core;

namespace NoteMapper.Services
{
    public interface IMusicTheoryService
    {
        Key? GetKey(string shortName);

        IReadOnlyCollection<string> GetKeyNames();        

        IReadOnlyCollection<Key> GetKeys();

        IReadOnlyCollection<string> GetKeyTypes();
    }
}
