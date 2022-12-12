using NoteMapper.Core.Instruments;

namespace NoteMapper.Web.Blazor.Models.Instruments
{
    public class InstrumentEditViewModel
    {
        private readonly List<InstrumentModifierViewModel> _modifiers = new();
        private readonly List<InstrumentStringViewModel> _strings = new();
        
        public InstrumentEditViewModel(string id, InstrumentType type)
        {
            Id = id;
            Type = type;
        }

        public string Id { get; }

        public IReadOnlyCollection<InstrumentModifierViewModel> Modifiers => _modifiers;

        public string Name { get; set; } = "";

        public IReadOnlyCollection<InstrumentStringViewModel> Strings => _strings;

        public InstrumentType Type { get; }

        public void AddModifier() => AddModifier(new());

        public void AddModifier(InstrumentModifierViewModel modifier)
        {
            _modifiers.Add(modifier);

            foreach (InstrumentStringViewModel s in _strings)
            {
                s.SetModifierOffsetCount(_modifiers.Count);
            }
        }

        public void AddString() => AddString(new());

        public void AddString(InstrumentStringViewModel @string)
        {
            @string.SetModifierOffsetCount(_modifiers.Count);

            _strings.Add(@string);
        }

        public void Reset()
        {
            _modifiers.Clear();
            _strings.Clear();
        }
    }
}
