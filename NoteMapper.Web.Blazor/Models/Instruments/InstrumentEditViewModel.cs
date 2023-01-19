using NoteMapper.Core.Extensions;
using NoteMapper.Core.Guitars;

namespace NoteMapper.Web.Blazor.Models.Instruments
{
    public class InstrumentEditViewModel
    {
        private readonly List<InstrumentModifierViewModel> _modifiers = new();
        private readonly List<InstrumentStringViewModel> _strings = new();
        
        public InstrumentEditViewModel(string id, GuitarType type)
        {
            Id = id;
            Type = type;
        }

        public string Id { get; }

        public IReadOnlyCollection<InstrumentModifierViewModel> Modifiers => _modifiers;

        public IReadOnlyCollection<string> ModifierTypes => Type.ModifierTypes().ToArray();

        public string Name { get; set; } = "";

        public IReadOnlyCollection<InstrumentStringViewModel> Strings => _strings;

        public GuitarType Type { get; }

        public void AddModifier() => AddModifier(new());

        public void AddModifier(InstrumentModifierViewModel modifier)
        {
            if (string.IsNullOrEmpty(modifier.Type))
            {
                modifier.Type = ModifierTypes.FirstOrDefault() ?? "";
            }

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

        public void MoveModifier(InstrumentModifierViewModel modifier, int direction)
        {
            _modifiers.MoveOne(modifier, direction);
        }

        public void RemoveModifier(InstrumentModifierViewModel modifier)
        {
            _modifiers.Remove(modifier);
        }

        public void RemoveString(InstrumentStringViewModel @string)
        {
            _strings.Remove(@string);
        }

        public void Reset()
        {
            _modifiers.Clear();
            _strings.Clear();
        }
    }
}
