using NoteMapper.Core.Guitars;
using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapNotesViewModel
    {
        public NoteMapNotesViewModel(IReadOnlyCollection<GuitarStringNote?> notes, Scale key, 
            AccidentalType accidental)
        {
            Notes = notes
                .Select(x => x != null ? new NoteMapNoteViewModel(x, key, accidental) : null)
                .ToArray();
        }

        public IReadOnlyCollection<NoteMapNoteViewModel?> Notes { get; }        

        public bool HasModifier(string name)
        {
            return Notes.Any(x => 
                string.Equals(x?.Modifier, name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
