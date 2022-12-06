using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using NoteMapper.Core.Extensions;

namespace NoteMapper.Core.MusicTheory
{
    public class Scale : NoteCollection
    {
        private static readonly Regex KeyRegex = new Regex(@"^(?<note>[A-Ga-g]#?)\s*?(?<type>.*)$", RegexOptions.Compiled);

        private static readonly IReadOnlyDictionary<KeyType, IReadOnlyCollection<byte>> KeyIntervals = CreateKeyIntervals();

        private static readonly IReadOnlyDictionary<KeyType, string> KeyShortNames = CreateKeyShortNames();

        private Scale(IEnumerable<Note> notes, KeyType type)
            : base(notes)
        {
            Type = type;
        }

        public KeyType Type { get; }

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
            IReadOnlyCollection<byte> intervals = GetIntervals(type);
            IEnumerable<Note> notes = GetNotes(note, intervals);
            return new(notes, type);
        }

        public static bool TryParse(string key, out Scale? scale)
        {
            try
            {
                scale = Parse(key);
                return true;
            }
            catch
            {
                scale = null;
                return false;
            }
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

        private static IReadOnlyDictionary<KeyType, IReadOnlyCollection<byte>> CreateKeyIntervals()
        {
            IDictionary<KeyType, IReadOnlyCollection<byte>> intervals = new Dictionary<KeyType, IReadOnlyCollection<byte>>
            {
                { KeyType.DominantSeven, new byte[] { 2, 2, 1, 2, 2, 1 } },
                { KeyType.Major, new byte[] { 2, 2, 1, 2, 2, 2 } },
                { KeyType.Minor, new byte[] { 2, 1, 2, 2, 1, 2 } }
            };

            return new ReadOnlyDictionary<KeyType, IReadOnlyCollection<byte>>(intervals);
        }

        private static IReadOnlyDictionary<KeyType, string> CreateKeyShortNames()
        {
            IDictionary<KeyType, string> keyShortNames = Enum.GetValues<KeyType>()
                .ToDictionary(x => x, x => x.ShortName());

            return new ReadOnlyDictionary<KeyType, string>(keyShortNames);
        }

        private static IReadOnlyCollection<byte> GetIntervals(KeyType type)
        {
            if (KeyIntervals.ContainsKey(type))
            {
                return KeyIntervals[type];
            }

            if (type.ToString().StartsWith(KeyType.Major.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return KeyIntervals[KeyType.Major];
            }

            if (type.ToString().StartsWith(KeyType.Minor.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return KeyIntervals[KeyType.Minor];
            }

            return Array.Empty<byte>();
        }

        private static IEnumerable<Note> GetNotes(Note startingNote, IReadOnlyCollection<byte> intervals)
        {
            yield return startingNote;

            foreach (byte interval in intervals)
            {
                yield return startingNote = startingNote.Next(interval);
            }
        }

        private static KeyType ParseKeyType(string type)
        {
            if (Enum.TryParse(type, out KeyType parsed) && Enum.IsDefined(parsed))
            {
                return parsed;
            }

            if (KeyShortNames.Any(x => string.Equals(type, x.Value, StringComparison.InvariantCultureIgnoreCase)))
            {
                return KeyShortNames
                    .First(x => string.Equals(type, x.Value, StringComparison.InvariantCultureIgnoreCase))
                    .Key;
            }

            return default;
        }
    }
}
