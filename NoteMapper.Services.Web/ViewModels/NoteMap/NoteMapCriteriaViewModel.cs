using NoteMapper.Core;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapCriteriaViewModel
    {
        public string? Instrument { get; set; }

        public string? KeyName { get; set; }

        public string? KeyType { get; set; }

        public NoteMapType Type { get; set; } = NoteMapType.Chord;
    }
}
