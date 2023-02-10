﻿using NoteMapper.Core.MusicTheory;
using NoteMapper.Core.NoteMap;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapCriteriaViewModel
    {
        public const AccidentalType DefaultAccidental = AccidentalType.Sharp;
        public const NoteMapMode DefaultMode = NoteMapMode.Permutations;

        public AccidentalType Accidental { get; set; } = DefaultAccidental;

        public string? InstrumentId { get; set; }

        public NoteMapMode Mode { get; set; } = DefaultMode;

        public int NoteIndex { get; set; }

        public string? ScaleType { get; set; }

        public bool ShowIntervals { get; set; }

        public NoteMapType Type { get; set; } = NoteMapType.Chord;
    }
}
