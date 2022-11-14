namespace NoteMapper.Core.Extensions
{
    public static class KeyTypeExtensions
    {
        public static string ShortName(this KeyType type)
        {
            switch (type)
            {
                case KeyType.DominantSeven:
                    return "7";
                case KeyType.MajorSeven:
                    return "maj7";
                case KeyType.Minor:
                    return "m";
                case KeyType.MinorSeven:
                    return "m7";
                default:
                    return "";
            }
        }
    }
}
