namespace NoteMapper.Web.Api.Models.Permutations.Responses
{
    public class PermutationsResponse
    {
        public PermutationsResponse(IEnumerable<PermutationString> strings)
        {
            Strings = strings.ToArray();
        }

        public PermutationString[] Strings { get; }
    }
}
