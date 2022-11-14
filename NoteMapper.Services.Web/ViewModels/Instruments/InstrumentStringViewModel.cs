using NoteMapper.Core.Instruments;

namespace NoteMapper.Services.Web.ViewModels.Instruments
{
    public class InstrumentStringViewModel
    {
        public InstrumentStringViewModel(InstrumentString @string)
        {
            Note = @string.OpenNote.Name;
            Positions = @string.Positions;
        }

        public int Positions { get; }

        public string Note { get; }
    }
}
