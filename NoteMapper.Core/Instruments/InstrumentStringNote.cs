namespace NoteMapper.Core.Instruments
{
    public class InstrumentStringNote
    {        
        public InstrumentStringNote(int position, InstrumentString @string, InstrumentStringModifier? modifier)
        {            
            Note = @string.NoteAt(position);
            Modifier = modifier;
            Position = position;
            String = @string;
        }

        public InstrumentStringModifier? Modifier { get; }

        public Note Note { get; }

        public int Position { get; }

        public InstrumentString String { get; }
    }
}
