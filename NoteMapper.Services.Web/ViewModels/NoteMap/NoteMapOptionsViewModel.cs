using NoteMapper.Core.MusicTheory;
using NoteMapper.Core.NoteMap;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapOptionsViewModel
    {
        public string Key { get; set; } = "";

        public NoteMapMode Mode { get; set; }

        public IReadOnlyCollection<string> Modifiers { get; set; } = Array.Empty<string>();

        public NoteMapType Type { get; set; }
    }
}
