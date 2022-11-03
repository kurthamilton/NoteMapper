using NoteMapper.Core.Instruments;

namespace NoteMapper.Web.Api.Models.Instruments.Responses
{
    public class ResponseModifier
    {
        public ResponseModifier(InstrumentStringModifier modifier)
        {
            Name = modifier.Name;
        }

        public string Name { get; }
    }
}
