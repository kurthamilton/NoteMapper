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

            IEnumerable<int> notes = GetChordNotes(scale.Type);

            IEnumerable<Note> chordNotes = notes
                .Select(x => scale.ElementAt(x));
            return new Chord(chordNotes);
        }

        private static IEnumerable<int> GetChordNotes(KeyType type)
        {
            switch (type)
            {
                case KeyType.Major:
                case KeyType.Minor:
                    return new[] { 0, 2, 4 };
                case KeyType.DominantSeven:
                case KeyType.MajorSeven:
                case KeyType.MinorSeven:
                    return GetChordNotes(KeyType.Major).Append(6);                    
                default:
                    return Enumerable.Empty<int>();
            }
        }
    }
}
