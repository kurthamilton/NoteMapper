using NoteMapper.Core.Instruments;

namespace NoteMapper.Web.Api.Models.Instruments.Responses
{
    public class ResponseInstrumentString
    {
        public ResponseInstrumentString(InstrumentString @string)
        {
            Note = new ResponseNote(@string.OpenNote);
        }

        public ResponseNote Note { get; }
    }
}
