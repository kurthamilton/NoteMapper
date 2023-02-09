using NoteMapper.Core.Extensions;
using NoteMapper.Core.Guitars;
using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Services.Web.ViewModels.Instruments
{
    public class InstrumentStringViewModel
    {
        private readonly List<StringOffsetViewModel> _modifierOffsets = new();
        
        public InstrumentStringViewModel(AccidentalType accidental)
        {
            Accidental = accidental;
        }

        public InstrumentStringViewModel(GuitarString @string, AccidentalType accidental)
            : this(accidental)
        {
            Frets = @string.Frets;
            NoteIndex = @string.OpenNote.NoteIndex;
            Octave = @string.OpenNote.OctaveIndex;
        }

        public AccidentalType Accidental { get; }

        public int Frets { get; set; }

        public IReadOnlyCollection<StringOffsetViewModel> ModifierOffsets => _modifierOffsets;

        public string Note => new Note(NoteIndex, 0).GetName(Accidental);

        public int NoteIndex { get; set; }

        public int Octave { get; set; }

        public void RemoveOffsets(int index)
        {
            _modifierOffsets.RemoveAt(index);
        }

        public void SetModifierOffsetCount(int count)
        {
            _modifierOffsets.SetCount(count, () => new());
        }

        public void SwitchOffsets(int oldIndex, int newIndex)
        {
            StringOffsetViewModel oldOffset = _modifierOffsets[oldIndex];
            StringOffsetViewModel newOffset = _modifierOffsets[newIndex];

            _modifierOffsets[oldIndex] = newOffset;
            _modifierOffsets[newIndex] = oldOffset;
        }
    }
}
