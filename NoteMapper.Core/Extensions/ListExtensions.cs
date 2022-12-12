namespace NoteMapper.Core.Extensions
{
    public static class ListExtensions
    {
        public static void SetCount<T>(this List<T> list, int count, T defaultValue)
        {
            list.SetCount(count, () => defaultValue);         
        }

        public static void SetCount<T>(this List<T> list, int count, Func<T> getDefaultValue)
        {
            for (int i = list.Count - 1; i >= count; i--)
            {
                list.RemoveAt(i);
            }

            while (list.Count < count)
            {
                list.Add(getDefaultValue());
            }
        }
    }
}
