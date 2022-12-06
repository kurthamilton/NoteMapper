using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Core.Instruments
{
    public class InstrumentStringNote
    {        
        public InstrumentStringNote(int position, InstrumentString @string, InstrumentStringModifier? modifier)
        {            
            IReadOnlyCollection<InstrumentStringModifier> modifiers = modifier != null 
                ? new[] { modifier } 
                : Array.Empty<InstrumentStringModifier>();

            Note = @string.NoteAt(position, modifiers);
            Modifier = modifier;
            Position = position;
            String = @string;
        }

        public InstrumentStringModifier? Modifier { get; }

        public Note Note { get; }

        public int Position { get; }

        public InstrumentString String { get; }

        public override string ToString()
        {
            return $"S: {String} | N: {Note} | M: {Modifier} | P: {Position}";
        }
    }
}
