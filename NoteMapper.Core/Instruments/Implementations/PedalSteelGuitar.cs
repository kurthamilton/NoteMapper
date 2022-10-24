using System.Transactions;
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

        public static PedalSteelGuitar Custom(IReadOnlyCollection<string> strings, IReadOnlyCollection<string> modifiers)
        {
            List<InstrumentStringModifier> modifierList = new List<InstrumentStringModifier>();
            foreach (string m in modifiers)
            {
                InstrumentStringModifier modifier = InstrumentStringModifier.Parse(m);
                modifierList.Add(modifier);
            }

            List<InstrumentString> stringList = new List<InstrumentString>();
            for (int i = 0; i < strings.Count; i++)
            {
                InstrumentString @string = InstrumentString.Parse(i, strings.ElementAt(i), modifierList);
                stringList.Add(@string);
            }

            return new PedalSteelGuitar(stringList, modifierList);
        }

        public static PedalSteelGuitar E9(int frets = 24)
        {
            return Custom(new[]
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
            }, new[]
            {
                "A:0+2,5+2", 
                "B:4+1,7+1", 
                "C:5+2,6+2", 
                "LKL:2-1,7-1", 
                "LKR:2+1,7+1", 
                "RKL:1-1,8-1", 
                "RKR:3+1,9+1"
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

        public IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote>> GetPermutations(Scale scale, int position)
        {
            DisableModifiers();

            // create set of note permutations without any modifiers applied
            List<List<InstrumentStringNote>> notePermutations = Strings
                .Select(x => new List<InstrumentStringNote>
                {
                    new InstrumentStringNote(position, x, null)
                })
                .ToList();

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

                    List<InstrumentStringNote> modifiedNotes = notePermutations[@string.Index];
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
