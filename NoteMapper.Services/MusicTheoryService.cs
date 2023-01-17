using NoteMapper.Core.Extensions;
using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Services
{
    public class MusicTheoryService : IMusicTheoryService
    {
        private static readonly IReadOnlyCollection<string> _keyTypes = Enum.GetValues<ScaleType>()
            .Select(x => x.ShortName())
            .ToArray();

        public Key? GetKey(string key)
        {
            IReadOnlyCollection<Key> keys = GetKeys();
            return keys
                .FirstOrDefault(x => string.Equals(x.ShortName, key, StringComparison.InvariantCultureIgnoreCase) || 
                                     string.Equals(x.Name, key, StringComparison.InvariantCultureIgnoreCase));
        }

        public IReadOnlyCollection<string> GetKeyNames()
        {
            return Note.GetNotes();
        }

        public IReadOnlyCollection<Key> GetKeys()
        {
            IReadOnlyCollection<string> notes = GetKeyNames();

            List<Key> keys = new();

            foreach (string note in notes)
            {
                foreach (string type in GetScaleTypes())
                {
                    Key key = new(note + type, $"{note} {type}");
                    keys.Add(key);
                }                
            }

            return keys;
        }

        public IReadOnlyCollection<string> GetScaleTypes()
        {
            return _keyTypes;
        }
    }
}
