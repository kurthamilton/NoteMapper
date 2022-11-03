using NoteMapper.Core.Instruments;

namespace NoteMapper.Web.Api.Models.Instruments.Responses
{
    public class ResponseInstrument
    {
        public ResponseInstrument(InstrumentBase instrument)
        {
            Name = instrument.Name;
            Type = instrument.Type;

            StringedInstrumentBase? stringedInstrument = instrument as StringedInstrumentBase;
            if (stringedInstrument != null)
            {
                Modifiers = stringedInstrument.Modifiers
                    .Select(x => new ResponseModifier(x))
                    .ToArray();
                
                Strings = stringedInstrument.Strings
                    .Select(x => new ResponseInstrumentString(x))
                    .ToArray();
            }            
        }

        public string Name { get; }

        public ResponseModifier[] Modifiers { get; } = Array.Empty<ResponseModifier>();

        public ResponseInstrumentString[] Strings { get; } = Array.Empty<ResponseInstrumentString>();

        public string Type { get; }
    }
}
