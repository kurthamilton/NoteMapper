using NoteMapper.Core.Permutations;

namespace NoteMapper.Core.Instruments.Implementations
{
    public class PedalSteelGuitar : StringedInstrumentBase
    {                
        private PedalSteelGuitar(string name, IEnumerable<InstrumentString> strings, InstrumentStringModifierCollection modifiers)
            : base(modifiers)
        {
            Name = name;
            Strings = strings.ToArray();
        }

        public override string Name { get; }

        public override IReadOnlyCollection<InstrumentString> Strings { get; }

        public override string Type => "PedalSteelGuitar";

        public static PedalSteelGuitar Custom(string name, PedalSteelGuitarConfig config)
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
            return new PedalSteelGuitar(name, strings, modifierCollection);
        }

        public static PedalSteelGuitar C6(int frets = 12)
        {
            return Custom("Pedal Steel C6", new PedalSteelGuitarConfig
            {
                Modifiers = new[]
                {
                    PedalSteelGuitarConfig.GetModifierConfig("4", 9 - 2, 2, 9 - 6, 2),
                    PedalSteelGuitarConfig.GetModifierConfig("5", 9 - 0, 2, 9 - 1, 1, 9 - 5, 1),
                    PedalSteelGuitarConfig.GetModifierConfig("6", 9 - 4, -1, 9 - 8, 1),
                    PedalSteelGuitarConfig.GetModifierConfig("7", 9 - 6, 2, 9 - 7, 2),
                    PedalSteelGuitarConfig.GetModifierConfig("8", 9 - 0, -3, 9 - 1, -1, 9 - 3, 1, 9 - 9, 1),
                    PedalSteelGuitarConfig.GetModifierConfig("LKL", 9 - 6, -1),
                    PedalSteelGuitarConfig.GetModifierConfig("LKR", 9 - 6, 1),
                    PedalSteelGuitarConfig.GetModifierConfig("RKL", 9 - 7, -1),
                    PedalSteelGuitarConfig.GetModifierConfig("RKR", 9 - 7, 1),
                },
                MutuallyExclusiveModifiers = new[]
                {
                    new KeyValuePair<string, string>("4", "6"),
                    new KeyValuePair<string, string>("4", "7"),
                    new KeyValuePair<string, string>("4", "8"),
                    new KeyValuePair<string, string>("5", "7"),
                    new KeyValuePair<string, string>("5", "8"),
                    new KeyValuePair<string, string>("6", "8"),
                    new KeyValuePair<string, string>("LKL", "LKR"),
                    new KeyValuePair<string, string>("RKL", "RKR")
                },
                Strings = new[]
                {
                    PedalSteelGuitarConfig.GetStringConfig("G3", frets),
                    PedalSteelGuitarConfig.GetStringConfig("E3", frets),
                    PedalSteelGuitarConfig.GetStringConfig("C3", frets),
                    PedalSteelGuitarConfig.GetStringConfig("A2", frets),
                    PedalSteelGuitarConfig.GetStringConfig("G2", frets),
                    PedalSteelGuitarConfig.GetStringConfig("E2", frets),
                    PedalSteelGuitarConfig.GetStringConfig("C2", frets),
                    PedalSteelGuitarConfig.GetStringConfig("A1", frets),
                    PedalSteelGuitarConfig.GetStringConfig("F1", frets),
                    PedalSteelGuitarConfig.GetStringConfig("C1", frets)
                }
            });
        }

        public static PedalSteelGuitar E9(int frets = 12)
        {
            return Custom("Pedal Steel E9", new PedalSteelGuitarConfig
            {
                Modifiers = new[]
                {
                    PedalSteelGuitarConfig.GetModifierConfig("A", 4, 2, 9, 2),
                    PedalSteelGuitarConfig.GetModifierConfig("B", 2, 1, 5, 1),
                    PedalSteelGuitarConfig.GetModifierConfig("C", 3, 2, 4, 2),
                    PedalSteelGuitarConfig.GetModifierConfig("LKL", 9 - 2, -1, 9 - 7, -1),
                    PedalSteelGuitarConfig.GetModifierConfig("LKR", 9 - 2, 1, 9 - 7, 1),
                    PedalSteelGuitarConfig.GetModifierConfig("RKL", 9 - 1, -1, 9 - 8, -1),
                    PedalSteelGuitarConfig.GetModifierConfig("RKR", 9 - 3, 1, 9 - 9, 1),
                },
                MutuallyExclusiveModifiers = new[]
                {
                    new KeyValuePair<string, string>("A", "C"),
                    new KeyValuePair<string, string>("LKL", "LKR"),
                    new KeyValuePair<string, string>("RKL", "RKR")
                },
                Strings = new[]
                {
                    PedalSteelGuitarConfig.GetStringConfig("F#4", frets),
                    PedalSteelGuitarConfig.GetStringConfig("D#4", frets),
                    PedalSteelGuitarConfig.GetStringConfig("G#4", frets),
                    PedalSteelGuitarConfig.GetStringConfig("E4", frets),
                    PedalSteelGuitarConfig.GetStringConfig("B3", frets),
                    PedalSteelGuitarConfig.GetStringConfig("G#3", frets),
                    PedalSteelGuitarConfig.GetStringConfig("F#3", frets),
                    PedalSteelGuitarConfig.GetStringConfig("E3", frets),
                    PedalSteelGuitarConfig.GetStringConfig("D3", frets),
                    PedalSteelGuitarConfig.GetStringConfig("B2", frets)
                }
            });
        }   
    }
}
