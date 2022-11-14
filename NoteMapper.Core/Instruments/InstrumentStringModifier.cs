using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using NoteMapper.Core.Permutations;

namespace NoteMapper.Core.Instruments
{
    public class InstrumentStringModifier
    {
        private static Regex _parseRegex = new Regex(@"^(?<name>\w+)\|(?<modifiers>.+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex _parseModifierRegex = new Regex(@"^(?<string>\d+)(?<offset>(\+|\-)\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);        

        private InstrumentStringModifier(string name, IDictionary<int, int> offsets)
        {
            Name = name;
            Offsets = new ReadOnlyDictionary<int, int>(offsets);
        }

        public string Name { get; }

        private IReadOnlyDictionary<int, int> Offsets { get; }

        private ICollection<InstrumentStringModifier> MutuallyExclusiveModifiers { get; } = new List<InstrumentStringModifier>();

        public static IDictionary<int, IReadOnlyCollection<InstrumentStringModifier>> GetPermutations(
            IReadOnlyCollection<InstrumentStringModifier> modifiers)
        {
            IDictionary<int, IReadOnlyCollection<InstrumentStringModifier>> modifierPermutations =
                    new Dictionary<int, IReadOnlyCollection<InstrumentStringModifier>>();

            foreach (Permutation permutation in Permutation.GetPermutations(modifiers.Count))
            {
                IReadOnlyCollection<InstrumentStringModifier> permutationModifiers = modifiers
                    .Where((modifier, i) => permutation.Get(i))
                    .ToArray();


            }

            return modifierPermutations;
        }

        public static InstrumentStringModifier Parse(string s)
        {
            Match match = _parseRegex.Match(s);
            if (!match.Success)
            {
                throw new ArgumentException("Invalid format", nameof(s));
            }

            string name = match.Groups["name"].Value;
            string modifierString = match.Groups["modifiers"].Value;
            string[] modifierStrings = modifierString.Split(',');            

            IDictionary<int, int> offsets = new Dictionary<int, int>();

            foreach (string m in modifierStrings)
            {
                Match modifierMatch = _parseModifierRegex.Match(m);
                if (!modifierMatch.Success)
                {
                    throw new ArgumentException("Invalid format", nameof(s));
                }

                int stringIndex = int.Parse(modifierMatch.Groups["string"].Value);
                int offset = int.Parse(modifierMatch.Groups["offset"].Value);

                offsets.Add(stringIndex, offset);
            }

            return new InstrumentStringModifier(name, offsets);
        }

        public int GetOffset(InstrumentString @string)
        {
            return Offsets.ContainsKey(@string.Index)
                ? Offsets[@string.Index]
                : 0;
        }

        public bool IsFor(int stringIndex)
        {
            return Offsets.ContainsKey(stringIndex);
        }

        public InstrumentStringModifier IsMutuallyExclusiveWith(InstrumentStringModifier other)
        {
            if (other == this)
            {
                return this;
            }

            if (MutuallyExclusiveModifiers.Contains(other))
            {
                return this;
            }

            MutuallyExclusiveModifiers.Add(other);
            other.IsMutuallyExclusiveWith(this);
            return this;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
