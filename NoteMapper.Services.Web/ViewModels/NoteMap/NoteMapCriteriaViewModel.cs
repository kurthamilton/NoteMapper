using NoteMapper.Core.MusicTheory;
using NoteMapper.Core.NoteMap;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapCriteriaViewModel
    {
        public const NoteMapMode DefaultMode = NoteMapMode.Permutations;

        public string? InstrumentId { get; set; }

        public string? KeyName { get; set; }

        public NoteMapMode Mode { get; set; } = DefaultMode;

        public string? ScaleType { get; set; }

        public bool ShowIntervals { get; set; }

        public NoteMapType Type { get; set; } = NoteMapType.Chord;
    }
}
