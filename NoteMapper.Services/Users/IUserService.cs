namespace NoteMapper.Services.Users
{
    public interface IUserService
    {
        Task<UserPreferences> GetPreferences(Guid? userId);
    }
}
