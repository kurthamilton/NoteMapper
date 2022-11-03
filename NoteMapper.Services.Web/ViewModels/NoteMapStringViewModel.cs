using NoteMapper.Core.Instruments;

namespace NoteMapper.Services.Web.ViewModels
{
    public class NoteMapStringViewModel
    {
        public NoteMapStringViewModel(InstrumentString @string)
        {
            Note = @string.OpenNote.Name;
            Positions = @string.Positions;
        }

        public int Positions { get; }

        public string Note { get; }
    }
}
