using NoteMapper.Core.Guitars;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapNotesViewModel
    {
        public NoteMapNotesViewModel(IReadOnlyCollection<GuitarStringNote?> notes)
        {
            Notes = notes;
        }

        public IReadOnlyCollection<GuitarStringNote?> Notes { get; }

        public bool HasModifier(string name)
        {
            return Notes.Any(x => 
                string.Equals(x?.Modifier?.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
