﻿using NoteMapper.Core.MusicTheory;
using NoteMapper.Core.NoteMap;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapOptionsViewModel
    {
        public AccidentalType Accidental { get; set; }

        public string Key { get; set; } = "";

        public NoteMapMode Mode { get; set; }

        public IReadOnlyCollection<string> Modifiers { get; set; } = Array.Empty<string>();

        public int NoteIndex { get; set; }

        public NoteMapType Type { get; set; }
    }
}
