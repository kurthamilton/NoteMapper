﻿namespace NoteMapper.Data.Core.Instruments
{
    public class UserInstrument
    {
        public int Frets { get; set; } = 12;

        public List<UserInstrumentModifier> Modifiers { get; set; } = new();

        public string Name { get; set; } = "";

        public List<UserInstrumentString> Strings { get; set; } = new();

        public string Type { get; set; } = "";

        public string UserInstrumentId { get; set; } = "";
    }
}
