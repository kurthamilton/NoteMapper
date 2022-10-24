using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc;
using NoteMapper.Core.Instruments;
using NoteMapper.Core.Instruments.Implementations;
using NoteMapper.Web.Api.Models.Permutations;

namespace NoteMapper.Web.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermutationsController : ControllerBase
    {        
        [HttpGet(Name = "{name}")]
        public PermutationsResponse? GetPermutations(string name, int position, string key)
        {
            StringedInstrumentBase? instrument = GetInstrument(name) as StringedInstrumentBase;
            if (instrument == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote>> permutations = 
                instrument.GetPermutations(key, position);

            PermutationsResponseString[] strings = permutations
                .Select(@string => new PermutationsResponseString
                {
                    Notes = @string.Select(x => new PermutationsResponseStringNote
                    {
                        Modifier = x.Modifier?.Name ?? "",
                        Name = x.Note.Name
                    }).ToArray()
                })
                .ToArray();

            PermutationsResponse response = new PermutationsResponse(strings);
            return response;
        }

        private InstrumentBase? GetInstrument(string instrument)
        {
            switch (instrument)
            {
                case "PedalSteelC6":
                    return PedalSteelGuitar.C6();
                case "PedalSteelE9":
                    return PedalSteelGuitar.E9();
                default:
                    return null;
            }
        }
    }
}