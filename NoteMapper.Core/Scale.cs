using System.Collections;
using System.Collections.ObjectModel;

namespace NoteMapper.Core
{
    public class Scale : IEnumerable<Note>
    {
        private static readonly string MajorIntervals = "221222";

        private readonly Lazy<IReadOnlyDictionary<int, Note>> _notes;

        private Scale(IEnumerable<Note> notes)
        {
            Notes = notes.ToArray();
            _notes = new Lazy<IReadOnlyDictionary<int, Note>>(() => 
                new ReadOnlyDictionary<int, Note>(Notes.ToDictionary(x => x.NoteIndex)));
        }

        public IReadOnlyCollection<Note> Notes { get; }

        public static Scale Major(string key)
        {
            Note note = Note.FromName(key);
            return Major(note);
        }

        public static Scale Major(Note key)
        {
            IEnumerable<Note> notes = ParseIntervals(key, MajorIntervals);
            return new Scale(notes);
        }        

        public bool Contains(Note note)
        {
            return Contains(note.NoteIndex);
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

        public IEnumerator<Note> GetEnumerator()
        {
            return Notes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

        private bool Contains(int noteIndex)
        {
            return _notes.Value.ContainsKey(noteIndex);
        }
    }
}
