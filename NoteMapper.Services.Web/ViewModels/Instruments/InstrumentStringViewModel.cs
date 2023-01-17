using NoteMapper.Core.Guitars;

namespace NoteMapper.Services.Web.ViewModels.Instruments
{
    public class InstrumentStringViewModel
    {
        public InstrumentStringViewModel(GuitarString @string)
        {
            Frets = @string.Frets;
            Note = @string.OpenNote.Name;
        }

        public int Frets { get; }

        public string Note { get; }
    }
}
