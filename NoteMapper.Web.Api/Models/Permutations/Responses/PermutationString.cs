namespace NoteMapper.Web.Api.Models.Permutations.Responses
{
    public class PermutationString
    {
        public PermutationString(IEnumerable<PermutationNote> notes)
        {
            Notes = notes.ToArray();
        }

        public PermutationNote[] Notes { get; }
    }
}
