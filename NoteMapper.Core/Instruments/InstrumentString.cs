using System.Text.RegularExpressions;

namespace NoteMapper.Core.Instruments
{
    public class InstrumentString
    {
        private static Regex _stringRegex = new Regex(@"^(?<note>[A-G]#?\d+)\|f=(?<startfret>\d+)\-(?<endfret>\d+)(|)?$", 
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public InstrumentString(int index, string openNote, int positions, IEnumerable<InstrumentStringModifier> modifiers)
        {
            Index = index;
            Positions = positions;

            Note[] notes = new Note[positions + 1];

            Note note = Note.Parse(openNote);
            OpenNote = note;
            notes[0] = note;

            for (int i = 1; i <= positions; i++)
            {
                note = note.Next(1);
                notes[i] = note;
            }

            Modifiers = modifiers.ToArray();
            Notes = notes;            
        }

        public InstrumentString(int index, string openNote, int positions)
            : this(index, openNote, positions, Array.Empty<InstrumentStringModifier>())
        {            
            
        }

        public int Index { get; }

        public IReadOnlyCollection<InstrumentStringModifier> Modifiers { get; }

        public IReadOnlyCollection<Note> Notes { get; }

        public Note OpenNote { get; }

        public int Positions { get; }

        public static InstrumentString Parse(int index, string s, IReadOnlyCollection<InstrumentStringModifier> modifiers)
        {
            Match match = _stringRegex.Match(s);
            if (!match.Success)
            {
                throw new ArgumentException("Invalid format", nameof(s));
            }

            string note = match.Groups["note"].Value;
            int startFret = int.Parse(match.Groups["startfret"].Value);
            int endFret = int.Parse(match.Groups["endfret"].Value);

            return new InstrumentString(index, note, endFret, modifiers.Where(x => x.IsFor(index)));
        }

        public bool HasModifier(InstrumentStringModifier modifier)
        {
            return Modifiers.Contains(modifier);
        }

        public Note NoteAt(int position, IReadOnlyCollection<InstrumentStringModifier> enabledModifiers)
        {
            if (position < 0 || 
                position > Positions)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            enabledModifiers = enabledModifiers
                .Where(x => HasModifier(x)).ToArray();

            int modifierOffset = enabledModifiers.Count > 0 ? enabledModifiers.Sum(x => x.GetOffset(this)) : 0;

            return Notes.ElementAt(position)
                .Next(modifierOffset);
        }

        public override string ToString()
        {
            return OpenNote.ToString();
        }
    }
}
