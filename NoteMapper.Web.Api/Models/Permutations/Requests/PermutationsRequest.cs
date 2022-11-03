using NoteMapper.Web.Api.Models.Instruments.Requests;

namespace NoteMapper.Web.Api.Models.Permutations.Requests
{
    public class PermutationsRequest
    {
        public string? Key { get; set; }

        public RequestInstrument Instrument { get; set; } = new RequestInstrument();

        public int Position { get; set; }        
    }
}
