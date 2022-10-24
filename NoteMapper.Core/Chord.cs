using System.Collections;

namespace NoteMapper.Core
{
    public class Chord : IEnumerable<Note>
    {
        private Chord(IEnumerable<Note> notes)
        {
            Notes = notes.ToArray();
        }        

        public IReadOnlyCollection<Note> Notes { get; }

        public static Chord Major(string key)
        {
            Note note = Note.FromName(key);
            return Major(note);
        }

        public static Chord Major(Note note)
        {            
            Scale scale = Scale.Major(note);
            return new Chord(new[]
            {
                scale.Notes.ElementAt(0),
                scale.Notes.ElementAt(2),
                scale.Notes.ElementAt(4)
            });
        }

        public IEnumerator<Note> GetEnumerator()
        {
            return Notes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
