using NoteMapper.Core.Extensions;
using NoteMapper.Core.Permutations;

namespace NoteMapper.Core.Instruments
{
    public abstract class StringedInstrumentBase : InstrumentBase
    {
        private readonly Lazy<int> _positions;

        protected StringedInstrumentBase(InstrumentStringModifierCollection modifiers)
        {
            Modifiers = modifiers;

            _positions = new Lazy<int>(() => Strings.Max(x => x.Positions));
        }

        public InstrumentStringModifierCollection Modifiers { get; }

        public int Positions => _positions.Value;

        public abstract IReadOnlyCollection<InstrumentString> Strings { get; }

        public IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote?>> GetPermutations(StringPermutationOptions options)
        {
            INoteCollection notes = options.Notes;

            List<IReadOnlyCollection<InstrumentStringNote?>> notePermutations = new();
            
            // Store an index of the modifier permutations that were used to avoid duplicating the
            // permutations containing redundant modifiers
            HashSet<int> usedPermutations = new();

            foreach (IReadOnlyCollection<InstrumentStringModifier> modifierPermutation in Modifiers.GetPermutations())
            {
                // the composition of the note, string, and possible modifier
                List<InstrumentStringNote?> stringNotes = new();
                // which modifier indexes have been used for this modifier permutation
                bool[] usedModifiers = new bool[Modifiers.Count];
                // which notes are being played in this permutation
                HashSet<int> usedNotes = new();

                for (int i = 0; i < Strings.Count; i++)
                {
                    // the current string
                    InstrumentString @string = Strings.ElementAt(i);
                    // get the first modifier from this permutation that applies to the current string
                    // it is assumed that modifiers are declared in order of precedence
                    InstrumentStringModifier? modifier = modifierPermutation
                        .FirstOrDefault(x => @string.HasModifier(x));
                    // the note being played on this string
                    Note note = @string.NoteAt(options.Position, 
                        modifier != null ? new[] { modifier } : Array.Empty<InstrumentStringModifier>());
                    if (notes.All(x => x.NoteIndex != note.NoteIndex))
                    {
                        // the note isn't in the current set of notes
                        stringNotes.Add(null);
                        continue;
                    }
                    
                    usedNotes.Add(note.NoteIndex);
                    
                    if (modifier != null)
                    {
                        int modifierIndex = Modifiers.IndexOf(modifier);
                        usedModifiers[modifierIndex] = true;
                    }

                    InstrumentStringNote stringNote = new(options.Position, @string, modifier);
                    stringNotes.Add(stringNote);
                }

                if (notes.Any(x => !usedNotes.Contains(x.NoteIndex)))
                {
                    // not all of the notes were found, do not use this permutation
                    continue;
                }
                
                Permutation permutation = new Permutation(usedModifiers);
                int permutationHashCode = permutation.GetHashCode();
                if (usedPermutations.Contains(permutationHashCode))
                {
                    // this combination of applied modifiers has already been used
                    // do not use this permutation as it is redundant
                    continue;
                }

                usedPermutations.Add(permutationHashCode);

                notePermutations.Add(stringNotes);
            }

            return notePermutations;
        }

        public IEnumerable<InstrumentStringModifier> AvailableModifiers(INoteCollection possibleNotes, int position)
        {
            foreach (InstrumentStringModifier modifier in Modifiers)
            {
                foreach (InstrumentString @string in Strings.Where(x => x.HasModifier(modifier)))
                {
                    Note note = @string.NoteAt(position, new[] { modifier });
                    if (possibleNotes.Contains(note))
                    {
                        yield return modifier;
                        break;
                    }
                }
            }
        }
    }
}
