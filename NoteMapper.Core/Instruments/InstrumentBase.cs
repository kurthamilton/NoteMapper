namespace NoteMapper.Core.Instruments
{
    public abstract class InstrumentBase
    {
        protected InstrumentBase()
        {
        }      
        
        public abstract string Id { get; }

        public abstract string Name { get; }

        public abstract InstrumentType Type { get; }
    }
}
