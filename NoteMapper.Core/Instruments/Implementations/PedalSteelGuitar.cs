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
                    PedalSteelGuitarConfig.GetModifierConfig("A", 9 - 0, 2, 9 - 5, 2),
                    PedalSteelGuitarConfig.GetModifierConfig("B", 9 - 4, 1, 9 - 7, 1),
                    PedalSteelGuitarConfig.GetModifierConfig("C", 9 - 5, 2, 9 - 6, 2),
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

        public bool Enable(params string[] modifierNames)
        {
            IReadOnlyCollection<InstrumentStringModifier?> modifiers = modifierNames
                .Select(x => Modifiers.FirstOrDefault(m => string.Equals(m.Name, x, StringComparison.InvariantCultureIgnoreCase)))
                .ToArray();

            List<InstrumentStringModifier> enabledModifiers = new List<InstrumentStringModifier>();

            foreach (InstrumentStringModifier? modifier in modifiers)
            {
                if (modifier == null)
                {
                    return false;
                }

                if (modifier.Enabled)
                {
                    continue;
                }

                if (!modifier.Enable())
                {
                    foreach (InstrumentStringModifier enabledModifier in enabledModifiers)
                    {
                        enabledModifier.Disable();
                    }

                    return false;
                }

                enabledModifiers.Add(modifier);
            }

            return true;
        }

        public override IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote>> GetPermutations(string key, int position, 
            NoteMapType type)
        {
            INoteCollection? notes = Note.GetNotes(type, key);
            if (notes == null)
            {
                return Array.Empty<InstrumentStringNote[]>();
            }

            // create set of note permutations without any modifiers applied
            IReadOnlyCollection<List<InstrumentStringNote>> notePermutations = Strings
                .Select(x => new List<InstrumentStringNote>())
                .ToList();
            for (int i = 0; i < Strings.Count; i++)
            {
                InstrumentString @string = Strings.ElementAt(i);
                Note note = @string.NoteAt(position, Array.Empty<InstrumentStringModifier>());
                if (!notes.Contains(note))
                {
                    continue;
                }

                notePermutations.ElementAt(i).Add(new InstrumentStringNote(position, @string, null));
            }

            IReadOnlyCollection<Permutation> permutations = Permutation.GetPermutations(Modifiers.Count);
            foreach (Permutation permutation in permutations)
            {                
                if (!EnableModifiers(permutation))
                {
                    continue;
                }

                foreach (InstrumentString @string in Strings)
                {                    
                    Note note = @string.NoteAt(position, Array.Empty<InstrumentStringModifier>());
                    if (!notes.Contains(note))
                    {
                        continue;
                    }

                    List<InstrumentStringNote> modifiedNotes = notePermutations.ElementAt(@string.Index);
                    IReadOnlyCollection<InstrumentStringModifier> modifiers = @string.Modifiers
                        .Where(x => x.Enabled)
                        .ToArray();

                    foreach (InstrumentStringModifier modifier in modifiers)
                    {
                        // add the modified note if it is the first time we have seen this note for this modifier
                        if (modifiedNotes.Any(x => x.Note.Index == note.Index && 
                                                   x.Modifier != null && x.Modifier.Name == modifier.Name))
                        {                            
                            continue;
                        }

                        InstrumentStringNote modifiedNote = new InstrumentStringNote(position, @string, modifier);
                        modifiedNotes.Add(modifiedNote);
                    }
                }
            }

            return notePermutations;
        }

        private void DisableModifiers()
        {
            foreach (InstrumentStringModifier modifier in Modifiers)
            {
                modifier.Disable();
            }
        }

        private bool EnableModifiers(Permutation permutation)
        {
            for (int i = 0; i < permutation.Count; i++)
            {
                bool value = permutation.Get(i);
                if (!value)
                {
                    continue;
                }

                InstrumentStringModifier modifier = Modifiers.ElementAt(i);
                if (!modifier.Enable())
                {
                    return false;
                }
            }

            return true;
        }        
    }
}
