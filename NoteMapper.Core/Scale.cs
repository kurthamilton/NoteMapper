using System.Text.RegularExpressions;

namespace NoteMapper.Core
{
    public class Scale : NoteCollection
    {
        private static readonly Regex KeyRegex = new Regex("^(?<note>[A-Ga-g]#?)(?<minor>m)?$", RegexOptions.Compiled);

        private static readonly string MajorIntervals = "221222";
        private static readonly string MinorIntervals = "212212";        

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
            bool minor = match.Groups["minor"].Success;

            return minor ? Minor(note) : Major(note);
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

        private static Scale Major(Note key)
        {
            IEnumerable<Note> notes = ParseIntervals(key, MajorIntervals);
            return new Scale(notes);
        }

        private static Scale Minor(Note key)
        {
            IEnumerable<Note> notes = ParseIntervals(key, MinorIntervals);
            return new Scale(notes);
        }

        private static IEnumerable<Note> ParseIntervals(Note startingNote, string intervals)
        {
            yield return startingNote;

            foreach (char c in intervals)
            {
                byte interval = byte.Parse(c.ToString());
                yield return startingNote = startingNote.Next(interval);
            }
        }        
    }
}
