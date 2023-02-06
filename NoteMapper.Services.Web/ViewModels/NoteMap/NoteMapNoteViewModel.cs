using NoteMapper.Core.Extensions;
using NoteMapper.Core.Guitars;
using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapNoteViewModel
    {
        public NoteMapNoteViewModel(GuitarStringNote note, Scale key)
        {
            Interval = key.GetInterval(note.Note.Name);
            Modifier = note.Modifier?.Name;
            Note = note.Note.Name;
        }

        public int Interval { get; set; }

        public string? Modifier { get; set; }

        public string Note { get; set; }
    }
}
