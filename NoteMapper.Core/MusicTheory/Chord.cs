namespace NoteMapper.Core.MusicTheory
{
    public class Chord : NoteCollection
    {
        private Chord(IEnumerable<Note> notes, Scale key)
            : base(notes)
        {
            Key = key;
        }

        public override Scale Key { get; }

        public static Chord Parse(int noteIndex, string key)
        {
            Scale scale = Scale.Parse(noteIndex, key);

            IEnumerable<int> notes = GetChordNotes(scale.Type);

            IEnumerable<Note> chordNotes = notes
                .Select(x => scale.ElementAt(x));
            return new Chord(chordNotes, scale);
        }

        private static IEnumerable<int> GetChordNotes(ScaleType type)
        {
            switch (type)
            {
                case ScaleType.Major:
                case ScaleType.Minor:
                    return new[] { 0, 2, 4 };
                case ScaleType.DominantSeven:
                case ScaleType.MajorSeven:
                case ScaleType.MinorSeven:
                    return GetChordNotes(ScaleType.Major).Append(6);
                default:
                    return Enumerable.Empty<int>();
            }
        }
    }
}
