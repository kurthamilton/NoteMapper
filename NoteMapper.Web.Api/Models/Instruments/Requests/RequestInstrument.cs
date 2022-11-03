namespace NoteMapper.Web.Api.Models.Instruments.Requests
{
    public class RequestInstrument
    {
        public RequestModifier[] Modifiers { get; set; } = Array.Empty<RequestModifier>();

        public string[][] MutuallyExclusiveModifiers { get; set; } = Array.Empty<string[]>();

        public string Name { get; set; } = "";

        public RequestString[] Strings { get; set; } = Array.Empty<RequestString>();
    }
}
