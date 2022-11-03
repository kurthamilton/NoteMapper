﻿namespace NoteMapper.Core.Instruments
{
    public abstract class InstrumentBase
    {
        protected InstrumentBase()
        {
        }      
        
        public abstract string Name { get; }

        public abstract string Type { get; }
    }
}
