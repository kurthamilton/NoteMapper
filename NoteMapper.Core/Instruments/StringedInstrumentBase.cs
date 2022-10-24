namespace NoteMapper.Core.Instruments
{
    public abstract class StringedInstrumentBase : InstrumentBase
    {
        public abstract IReadOnlyCollection<InstrumentString> Strings { get; }

        public abstract IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote>> GetPermutations(string key, int position);
    }
}
