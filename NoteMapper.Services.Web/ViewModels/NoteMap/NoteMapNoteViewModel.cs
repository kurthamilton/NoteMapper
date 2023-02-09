using NoteMapper.Core.Guitars;
using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapNoteViewModel
    {
        public NoteMapNoteViewModel(GuitarStringNote note, Scale key, AccidentalType accidental)
        {
            Interval = key.GetInterval(note.Note);
            Modifier = note.Modifier?.Name;
            Note = note.Note.GetName(accidental);
        }

        public int Interval { get; set; }

        public string? Modifier { get; set; }

        public string Note { get; set; }
    }
}
