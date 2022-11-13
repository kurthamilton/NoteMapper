namespace NoteMapper.Core.Extensions
{
    public static class KeyTypeExtensions
    {
        public static string ShortName(this KeyType type)
        {
            switch (type)
            {
                case KeyType.Minor:
                    return "m";
                default:
                    return "";
            }
        }
    }
}
