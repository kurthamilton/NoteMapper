using NoteMapper.Core;

namespace NoteMapper.Services
{
    public class MusicTheoryService : IMusicTheoryService
    {
        public Key? GetKey(string shortName)
        {
            IReadOnlyCollection<Key> keys = GetKeys();
            return keys
                .FirstOrDefault(x => string.Equals(x.ShortName, shortName, StringComparison.InvariantCultureIgnoreCase));
        }

        public IReadOnlyCollection<Key> GetKeys()
        {
            IReadOnlyCollection<string> notes = Note.GetNotes();

            List<Key> keys = new();

            // major keys
            foreach (string note in notes)
            {
                Key key = new(note, $"{note} Major");
                keys.Add(key);
            }

            // minor keys
            foreach (string note in notes)
            {
                Key key = new($"{note}m", $"{note} Major");
                keys.Add(key);
            }

            return keys;
        }
    }
}
