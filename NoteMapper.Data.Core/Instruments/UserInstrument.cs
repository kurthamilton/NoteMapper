namespace NoteMapper.Data.Core.Instruments
{
    public class UserInstrument
    {
        public int? Frets { get; set; }

        public IReadOnlyCollection<UserInstrumentModifier> Modifiers { get; set; } = Array.Empty<UserInstrumentModifier>();

        public string Name { get; set; } = "";

        public IReadOnlyCollection<UserInstrumentString> Strings { get; set; } = Array.Empty<UserInstrumentString>();

        public string Type { get; set; } = "";
    }
}
