using NoteMapper.Core.Permutations;

namespace NoteMapper.Core.Instruments
{
    public abstract class StringedInstrumentBase : InstrumentBase
    {
        protected StringedInstrumentBase(InstrumentStringModifierCollection modifiers)
        {
            Modifiers = modifiers;
        }

        public InstrumentStringModifierCollection Modifiers { get; }

        public abstract IReadOnlyCollection<InstrumentString> Strings { get; }

        public abstract IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote>> GetPermutations(string key, int position,
            NoteMapType type);

        public IEnumerable<InstrumentStringModifier> AvailableModifiers(INoteCollection possibleNotes, int position)
        {
            foreach (InstrumentStringModifier modifier in Modifiers)
            {
                if (!modifier.CanEnable() || modifier.Enabled)
                {
                    continue;
                }

                modifier.Enable();

                foreach (InstrumentString @string in Strings.Where(x => x.HasModifier(modifier)))
                {
                    Note note = @string.NoteAt(position, new[] { modifier });
                    if (possibleNotes.Contains(note))
                    {
                        yield return modifier;
                        break;
                    }
                }

                modifier.Disable();
            }
        }

        protected IEnumerable<InstrumentStringModifier> GetModifierPermutation(Permutation permutation)
        {
            throw new NotImplementedException();
        }
    }
}
