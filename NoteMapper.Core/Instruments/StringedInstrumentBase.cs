namespace NoteMapper.Core.Instruments
{
    public abstract class StringedInstrumentBase
    {
        public abstract IReadOnlyCollection<InstrumentString> Strings { get; }
    }
}
