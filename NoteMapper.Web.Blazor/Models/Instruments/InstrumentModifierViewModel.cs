namespace NoteMapper.Web.Blazor.Models.Instruments
{
    public class InstrumentModifierViewModel
    {
        public List<InstrumentModifierViewModel> IncompatibleModifiers { get; } = new();

        public string Name { get; set; } = "";

        public int? OriginalIndex { get; set; }

        public string Type { get; set; } = "";
    }
}
