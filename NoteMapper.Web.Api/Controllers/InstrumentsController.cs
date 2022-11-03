using Microsoft.AspNetCore.Mvc;
using NoteMapper.Core.Instruments;
using NoteMapper.Core.Instruments.Implementations;
using NoteMapper.Services;
using NoteMapper.Web.Api.Models.Instruments.Requests;
using NoteMapper.Web.Api.Models.Instruments.Responses;
using NoteMapper.Web.Api.Models.Permutations.Requests;
using NoteMapper.Web.Api.Models.Permutations.Responses;

namespace NoteMapper.Web.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InstrumentsController : ControllerBase
    {
        private readonly IInstrumentFactory _instrumentFactory;

        public InstrumentsController(IInstrumentFactory instrumentFactory)
        {
            _instrumentFactory = instrumentFactory;
        }

        [HttpGet]
        [Route("")]
        public InstrumentsResponse GetInstruments()
        {
            IReadOnlyCollection<InstrumentBase> instruments = _instrumentFactory.GetInstruments();
            return new InstrumentsResponse(instruments);
        }
    }
}