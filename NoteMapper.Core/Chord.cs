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

        public static Chord Parse(string key)
        {
            Scale scale = Scale.Parse(key);
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
