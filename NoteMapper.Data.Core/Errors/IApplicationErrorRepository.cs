namespace NoteMapper.Data.Core.Errors
{
    public interface IApplicationErrorRepository
    {
        Task CreateAsync(ApplicationError error);
    }
}
