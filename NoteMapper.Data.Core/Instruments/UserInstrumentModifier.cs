namespace NoteMapper.Data.Core.Instruments
{
    public class UserInstrumentModifier
    {
        public IReadOnlyCollection<string>? MutuallyExclusive { get; set; }

        public string Name { get; set; } = "";

        public IReadOnlyCollection<ModifierOffset> Offsets { get; set; } = Array.Empty<ModifierOffset>();

        public string Type { get; set; } = "";
    }
}
