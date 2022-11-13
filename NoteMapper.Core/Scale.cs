using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace NoteMapper.Core
{
    public class Scale : NoteCollection
    {
        private static readonly Regex KeyRegex = new Regex(@"^(?<note>[A-Ga-g]#?)\s*?(?<type>.*)$", RegexOptions.Compiled);

        private static readonly IReadOnlyDictionary<KeyType, IReadOnlyCollection<byte>> KeyIntervals = GetKeyIntervals();

        private Scale(IEnumerable<Note> notes)
            : base(notes)
        {            
        }

        public static Scale Parse(string key)
        {
            Match match = KeyRegex.Match(key);
            if (!match.Success)
            {
                throw new ArgumentException();
            }

            Note note = Note.Parse(match.Groups["note"].Value);
            Group typeGroup = match.Groups["type"];
            KeyType type = ParseKeyType(typeGroup.Success ? typeGroup.Value : "");
            IEnumerable<Note> notes = ParseIntervals(note, KeyIntervals[type]);
            return new(notes);
        }

        public IEnumerable<Note> NotesBetween(int start, int end)
        {
            while (start <= end)
            {
                int noteIndex = Note.GetNoteIndex(start);
                if (Contains(noteIndex))
                {
                    yield return new Note(start);
                }

                start++;
            }
        }

        public IEnumerable<Note> Overlapping(Scale other)
        {
            foreach (Note note in other)
            {
                if (Contains(note))
                {
                    yield return note;
                }
            }
        }        

        private static IReadOnlyDictionary<KeyType, IReadOnlyCollection<byte>> GetKeyIntervals()
        {
            IDictionary<KeyType, IReadOnlyCollection<byte>> intervals = new Dictionary<KeyType, IReadOnlyCollection<byte>>
            {
                { KeyType.Major, new byte[] { 2, 2, 1, 2, 2, 2 } },
                { KeyType.Minor, new byte[] { 2, 1, 2, 2, 1, 2 } }
            };

            return new ReadOnlyDictionary<KeyType, IReadOnlyCollection<byte>>(intervals);
        }

        private static IEnumerable<Note> ParseIntervals(Note startingNote, IReadOnlyCollection<byte> intervals)
        {
            yield return startingNote;

            foreach (byte interval in intervals)
            {
                yield return startingNote = startingNote.Next(interval);
            }
        }  
        
        private static KeyType ParseKeyType(string type)
        {
            return Enum.TryParse(type, out KeyType parsed)
                ? parsed
                : default;
        }
    }
}
