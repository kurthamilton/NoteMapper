namespace NoteMapper.Core.Instruments.Implementations
{
    public class PedalSteelGuitarConfig
    {
        public IReadOnlyCollection<string> Modifiers { get; set; } = Array.Empty<string>();

        public IReadOnlyCollection<KeyValuePair<string, string>> MutuallyExclusiveModifiers { get; set; } 
            = Array.Empty<KeyValuePair<string, string>>();

        public IReadOnlyCollection<string> Strings { get; set; } = Array.Empty<string>();
    }
}
