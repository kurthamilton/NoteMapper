namespace NoteMapper.Web.Api.Models.Instruments.Requests
{
    public class RequestModifier
    {
        public string Name { get; set; } = "";

        public RequestModifierOffset[] Offsets { get; set; } = Array.Empty<RequestModifierOffset>();
    }
}
