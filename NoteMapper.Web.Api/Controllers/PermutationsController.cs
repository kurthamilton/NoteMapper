using Microsoft.AspNetCore.Mvc;
using NoteMapper.Core.Instruments;
using NoteMapper.Core.Instruments.Implementations;
using NoteMapper.Services;
using NoteMapper.Web.Api.Models.Instruments.Requests;
using NoteMapper.Web.Api.Models.Permutations.Requests;
using NoteMapper.Web.Api.Models.Permutations.Responses;

namespace NoteMapper.Web.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermutationsController : ControllerBase
    {
        private readonly IInstrumentFactory _instrumentFactory;

        public PermutationsController(IInstrumentFactory instrumentFactory)
        {
            _instrumentFactory = instrumentFactory;
        }

        [HttpPost]
        [Route("")]
        public PermutationsResponse? GetPermutations(PermutationsRequest request)
        {
            StringedInstrumentBase? instrument = GetInstrument(request.Instrument) as StringedInstrumentBase;
            if (instrument == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote>> permutations =
                instrument.GetPermutations(request.Key ?? "", request.Position);

            PermutationsResponse response = new PermutationsResponse(permutations
                .Select(@string => new PermutationString(@string.Select(x => new PermutationNote
                {
                    Modifier = x.Modifier?.Name ?? "",
                    Name = x.Note.Name
                }))));
            return response;
        }

        private InstrumentBase? GetInstrument(RequestInstrument request)
        {
            InstrumentBase? instrument = _instrumentFactory.GetInstrument(request.Name);
            if (instrument != null)
            {
                return instrument;
            }

            switch (request.Name)
            {
                case "PedalSteelGuitar":
                    return PedalSteelGuitar.Custom(request.Name, new PedalSteelGuitarConfig
                    {
                        Modifiers = request.Modifiers
                            .Select(x =>
                            {
                                int[] offsets = new int[x.Offsets.Length * 2];
                                for (int i = 0; i < x.Offsets.Length; i++)
                                {
                                    offsets[i * 2] = x.Offsets[i].StringIndex;
                                    offsets[i * 2 + 1] = x.Offsets[i].Offset;
                                }

                                return PedalSteelGuitarConfig.GetModifierConfig(x.Name, offsets);
                            })
                            .ToArray(),
                        MutuallyExclusiveModifiers = request.MutuallyExclusiveModifiers
                            .Select(x => new KeyValuePair<string, string>(x[0], x[1]))
                            .ToArray(),
                        Strings = request.Strings
                            .Select(x => PedalSteelGuitarConfig.GetStringConfig(x.Note, x.Frets))
                            .ToArray()
                    });                
                default:
                    return null;
            }
        }
    }
}