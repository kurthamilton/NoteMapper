using NoteMapper.Core.Extensions;
using NoteMapper.Core.Guitars;

namespace NoteMapper.Services.Web.ViewModels.Instruments
{
    public class InstrumentStringViewModel
    {
        private readonly List<StringOffsetViewModel> _modifierOffsets = new();

        public InstrumentStringViewModel()
        {
        }

        public InstrumentStringViewModel(GuitarString @string)
        {
            Frets = @string.Frets;
            Note = @string.OpenNote.Name;
            Octave = @string.OpenNote.OctaveIndex;
        }
        public int Frets { get; set; }

        public IReadOnlyCollection<StringOffsetViewModel> ModifierOffsets => _modifierOffsets;

        public string Note { get; set; } = "";

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
