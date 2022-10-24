namespace NoteMapper.Web.Api.Models.Permutations
{
    public class PermutationsResponse
    {
        public PermutationsResponse(PermutationsResponseString[] strings)
        {
            Strings = strings;
        }

        public PermutationsResponseString[] Strings { get; }
    }
}
