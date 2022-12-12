using NoteMapper.Core.Permutations;

namespace NoteMapper.Core.Instruments.Implementations
{
    public class PedalSteelGuitar : StringedInstrumentBase
    {
        private PedalSteelGuitar(string id, string name, IEnumerable<InstrumentString> strings, 
            InstrumentStringModifierCollection modifiers)
            : base(modifiers)
        {
            Id = id;
            Name = name;
            Strings = strings.ToArray();
        }

        public override string Id { get; }

        public override string Name { get; }

        public override IReadOnlyCollection<InstrumentString> Strings { get; }

        public override InstrumentType Type => InstrumentType.PedalSteelGuitar;

        public static PedalSteelGuitar Custom(string id, string name, PedalSteelGuitarConfig config)
        {
            List<InstrumentStringModifier> modifiers = new List<InstrumentStringModifier>();
            foreach (string m in config.Modifiers)
            {
                InstrumentStringModifier modifier = InstrumentStringModifier.Parse(m);
                modifiers.Add(modifier);
            }

            List<InstrumentString> strings = new List<InstrumentString>();
            for (int i = 0; i < config.Strings.Count; i++)
            {
                InstrumentString @string = InstrumentString.Parse(i, config.Strings.ElementAt(i), modifiers);
                strings.Add(@string);
            }

            InstrumentStringModifierCollection modifierCollection = new InstrumentStringModifierCollection(modifiers,
                config.MutuallyExclusiveModifiers);
            return new PedalSteelGuitar(id, name, strings, modifierCollection);
        }
    }
}
