namespace NoteMapper.Services.Web.StateManagement
{
    public interface IStateContainer
    {
        T? GetTempData<T>(string key);

        string SetTempData<T>(T obj);
    }
}
