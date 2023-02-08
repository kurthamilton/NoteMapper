using System.Text.RegularExpressions;
using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Core.Guitars
{
    public class GuitarString
    {
        private static Regex _stringRegex = new(@"^(?<note>[A-G]#?\d+)\|f=(?<startfret>\d+)\-(?<endfret>\d+)(|)?$", 
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public GuitarString(int index, string openNote, int fret, IEnumerable<GuitarStringModifier> modifiers)
        {
            Index = index;
            Frets = fret;

            Note note = Note.Parse(openNote);
            Modifiers = modifiers.ToArray();           
            OpenNote = note;            
        }

        public GuitarString(int index, string openNote, int frets)
            : this(index, openNote, frets, Array.Empty<GuitarStringModifier>())
        {                        
        }

        public int Frets { get; }

        public int Index { get; }        

        public Note OpenNote { get; }

        private IReadOnlyCollection<GuitarStringModifier> Modifiers { get; }

        public static GuitarString Parse(int index, string s, IReadOnlyCollection<GuitarStringModifier> modifiers)
        {
            Match match = _stringRegex.Match(s);
            if (!match.Success)
            {
                throw new ArgumentException("Invalid format", nameof(s));
            }

            string note = match.Groups["note"].Value;
            int startFret = int.Parse(match.Groups["startfret"].Value);
            int endFret = int.Parse(match.Groups["endfret"].Value);

            return new GuitarString(index, note, endFret, modifiers.Where(x => x.IsFor(index)));
        }

        public bool HasModifier(GuitarStringModifier modifier)
        {
            return Modifiers.Contains(modifier);
        }

        public Note NoteAt(int fret, IReadOnlyCollection<GuitarStringModifier> enabledModifiers)
        {
            if (fret < 0 || 
                fret > Frets)
            {
                throw new ArgumentOutOfRangeException(nameof(fret));
            }

            enabledModifiers = enabledModifiers
                .Where(x => HasModifier(x)).ToArray();

            int modifierOffset = enabledModifiers.Count > 0 ? enabledModifiers.Sum(x => x.GetOffset(this)) : 0;

            return OpenNote
                .Next(fret)
                .Next(modifierOffset);
        }

        public override string ToString()
        {
            return OpenNote.ToString();
        }
    }
}
