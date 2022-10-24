using NoteMapper.Core.Permutations;

namespace NoteMapper.Core.Instruments.Implementations
{
    public class PedalSteelGuitar : StringedInstrumentBase
    {                
        private PedalSteelGuitar(IEnumerable<InstrumentString> strings, IEnumerable<InstrumentStringModifier> modifiers)
        {
            Strings = strings.ToArray();
            Modifiers = modifiers.ToArray();
        }

        public IReadOnlyCollection<InstrumentStringModifier> Modifiers { get; }

        public override IReadOnlyCollection<InstrumentString> Strings { get; }

        public static PedalSteelGuitar Custom(PedalSteelGuitarConfig config)
        {
            List<InstrumentStringModifier> modifiers = new List<InstrumentStringModifier>();
            foreach (string m in config.Modifiers)
            {
                InstrumentStringModifier modifier = InstrumentStringModifier.Parse(m);
                modifiers.Add(modifier);
            }

            foreach (KeyValuePair<string, string> pair in config.MutuallyExclusiveModifiers)
            {
                InstrumentStringModifier? modifier1 = modifiers
                    .FirstOrDefault(x => string.Equals(x.Name, pair.Key, StringComparison.InvariantCultureIgnoreCase));
                InstrumentStringModifier? modifier2 = modifiers
                    .FirstOrDefault(x => string.Equals(x.Name, pair.Value, StringComparison.InvariantCultureIgnoreCase));

                if (modifier1 == null || modifier2 == null)
                {
                    throw new ArgumentException("Mutually exclusive modifier not found", 
                        nameof(config.MutuallyExclusiveModifiers));
                }

                modifier1.IsMutuallyExclusiveWith(modifier2);
            }

            List<InstrumentString> strings = new List<InstrumentString>();
            for (int i = 0; i < config.Strings.Count; i++)
            {
                InstrumentString @string = InstrumentString.Parse(i, config.Strings.ElementAt(i), modifiers);
                strings.Add(@string);
            }

            return new PedalSteelGuitar(strings, modifiers);
        }

        public static PedalSteelGuitar C6(int frets = 24)
        {
            return Custom(new PedalSteelGuitarConfig
            {
                Modifiers = new[]
                {
                    "4|2+2,6+2",
                    "5|0+2,1+1,5-1",
                    "6|4-1,8+1",
                    "7|6+2,7+2",
                    "8|0-3,1-1,3+1,9+1",
                    "LKL|6-1",
                    "LKR|6+1",
                    "RKL|7-1",
                    "RKR|7+1"
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
                    $"C1|f=0-{frets}",
                    $"F1|f=0-{frets}",
                    $"A1|f=0-{frets}",
                    $"C2|f=0-{frets}",
                    $"E2|f=0-{frets}",
                    $"G2|f=0-{frets}",
                    $"A2|f=0-{frets}",
                    $"C3|f=0-{frets}",
                    $"E3|f=0-{frets}",
                    $"G3|f=0-{frets}"
                }
            });
        }

        public static PedalSteelGuitar E9(int frets = 24)
        {
            return Custom(new PedalSteelGuitarConfig
            {
                Modifiers = new[]
                {
                    "A|0+2,5+2",
                    "B|4+1,7+1",
                    "C|5+2,6+2",
                    "LKL|2-1,7-1",
                    "LKR|2+1,7+1",
                    "RKL|1-1,8-1",
                    "RKR|3+1,9+1"
                },
                MutuallyExclusiveModifiers = new[]
                {
                    new KeyValuePair<string, string>("A", "C"),
                    new KeyValuePair<string, string>("LKL", "LKR"),
                    new KeyValuePair<string, string>("RKL", "RKR")
                },
                Strings = new[]
                {
                    $"B2|f=0-{frets}",
                    $"D3|f=0-{frets}",
                    $"E3|f=0-{frets}",
                    $"F#3|f=0-{frets}",
                    $"G#3|f=0-{frets}",
                    $"B3|f=0-{frets}",
                    $"E4|f=0-{frets}",
                    $"G#4|f=0-{frets}",
                    $"D#4|f=0-{frets}",
                    $"F#4|f=0-{frets}"
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

        public override IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote>> GetPermutations(string key, int position)
        {
            Scale scale = Scale.Parse(key);

            DisableModifiers();

            // create set of note permutations without any modifiers applied
            IReadOnlyCollection<List<InstrumentStringNote>> notePermutations = Strings
                .Select(x => new List<InstrumentStringNote>())
                .ToList();
            for (int i = 0; i < Strings.Count; i++)
            {
                InstrumentString @string = Strings.ElementAt(i);
                Note note = @string.NoteAt(position);
                if (!note.InScale(scale))
                {
                    continue;
                }

                notePermutations.ElementAt(i).Add(new InstrumentStringNote(position, @string, null));
            }

            IReadOnlyCollection<Permutation> permutations = Permutation.GetPermutations(Modifiers.Count);
            foreach (Permutation permutation in permutations)
            {
                DisableModifiers();

                if (!EnableModifiers(permutation))
                {
                    continue;
                }

                foreach (InstrumentString @string in Strings)
                {                    
                    Note note = @string.NoteAt(position);
                    if (!scale.Contains(note))
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
