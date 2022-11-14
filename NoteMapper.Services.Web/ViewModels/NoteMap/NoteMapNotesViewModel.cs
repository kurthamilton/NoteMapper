using NoteMapper.Core.Instruments;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapNotesViewModel
    {
        public NoteMapNotesViewModel(IReadOnlyCollection<InstrumentStringNote?> notes)
        {
            Notes = notes;
        }

        public IReadOnlyCollection<InstrumentStringNote?> Notes { get; }

        public bool HasModifier(string name)
        {
            return Notes.Any(x => 
                string.Equals(x?.Modifier?.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
