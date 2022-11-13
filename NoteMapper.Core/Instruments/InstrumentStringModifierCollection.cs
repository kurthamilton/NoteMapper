using System.Collections;
using NoteMapper.Core.Extensions;
using NoteMapper.Core.Permutations;

namespace NoteMapper.Core.Instruments
{
    public class InstrumentStringModifierCollection : IReadOnlyCollection<InstrumentStringModifier>
    {        
        private readonly IReadOnlyCollection<InstrumentStringModifier> _modifiers;
        private readonly IReadOnlyCollection<KeyValuePair<string, string>> _mutuallyExclusive;
        private IReadOnlyCollection<IReadOnlyCollection<InstrumentStringModifier>>? _permutations;

        public InstrumentStringModifierCollection(IEnumerable<InstrumentStringModifier> modifiers,
            IEnumerable<KeyValuePair<string, string>> mutuallyExclusive)
        {
            _modifiers = modifiers.ToArray();
            _mutuallyExclusive = mutuallyExclusive.ToArray();
        }

        public int Count => _modifiers.Count;

        public IEnumerator<InstrumentStringModifier> GetEnumerator()
        {
            return _modifiers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IReadOnlyCollection<IReadOnlyCollection<InstrumentStringModifier>> GetPermutations()
        {
            if (_permutations != null)
            {
                return _permutations;
            }

            List<IReadOnlyCollection<InstrumentStringModifier>> modifierPermutations = new();

            HashSet<Permutation> invalidPermutations = new();
            foreach (KeyValuePair<string, string> pair in _mutuallyExclusive)
            {
                InstrumentStringModifier? modifier1 = _modifiers
                    .FirstOrDefault(x => string.Equals(x.Name, pair.Key, StringComparison.InvariantCultureIgnoreCase));
                InstrumentStringModifier? modifier2 = _modifiers
                    .FirstOrDefault(x => string.Equals(x.Name, pair.Value, StringComparison.InvariantCultureIgnoreCase));

                if (modifier1 == null || modifier2 == null)
                {
                    continue;
                }                

                modifier1.IsMutuallyExclusiveWith(modifier2);

                bool[] bits = new bool[_modifiers.Count];
                bits[_modifiers.IndexOf(modifier1)] = true;
                bits[_modifiers.IndexOf(modifier2)] = true;

                invalidPermutations.Add(new Permutation(bits));
            }

            IReadOnlyCollection<Permutation> permutations = Permutation.GetPermutations(_modifiers.Count);
            foreach (Permutation permutation in permutations)
            {
                if (invalidPermutations.Any(x => permutation.Contains(x)))
                {
                    continue;
                }

                IReadOnlyCollection<InstrumentStringModifier> permutationModifiers = _modifiers
                    .Where((modifier, i) => permutation.Get(i))
                    .ToArray();
                modifierPermutations.Add(permutationModifiers);
            }

            _permutations = modifierPermutations;

            return _permutations;
        }
    }
}
