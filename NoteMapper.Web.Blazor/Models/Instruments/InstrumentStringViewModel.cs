using NoteMapper.Core.Extensions;

namespace NoteMapper.Web.Blazor.Models.Instruments
{
    public class InstrumentStringViewModel
    {
        private readonly List<StringOffsetViewModel> _modifierOffsets = new();

        public IReadOnlyCollection<StringOffsetViewModel> ModifierOffsets => _modifierOffsets;

        public string Note { get; set; } = "";

        public int Octave { get; set; }        

        public void SetModifierOffsetCount(int count)
        {
            _modifierOffsets.SetCount(count, () => new());
        }
    }
}
