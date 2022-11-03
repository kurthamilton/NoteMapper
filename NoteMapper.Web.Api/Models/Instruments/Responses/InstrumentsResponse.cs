using NoteMapper.Core.Instruments;

namespace NoteMapper.Web.Api.Models.Instruments.Responses
{
    public class InstrumentsResponse
    {
        public InstrumentsResponse(IEnumerable<InstrumentBase> instruments)
        {
            Instruments = instruments
                .Select(x => new ResponseInstrument(x))
                .ToArray();
        }

        public ResponseInstrument[] Instruments { get; }
    }
}
