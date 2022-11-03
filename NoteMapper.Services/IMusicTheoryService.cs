using NoteMapper.Core;

namespace NoteMapper.Services
{
    public interface IMusicTheoryService
    {
        Key? GetKey(string shortName);

        IReadOnlyCollection<Key> GetKeys();
    }
}
