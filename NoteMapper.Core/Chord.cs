namespace NoteMapper.Core
{
    public class Chord : NoteCollection
    {
        private Chord(IEnumerable<Note> notes)
            : base(notes)
        {
        }

        public static Chord Parse(string key)
        {
            Scale scale = Scale.Parse(key);
            return new Chord(new[]
            {
                scale.ElementAt(0),
                scale.ElementAt(2),
                scale.ElementAt(4)
            });
        }
    }
}
