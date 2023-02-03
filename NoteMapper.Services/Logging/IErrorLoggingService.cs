namespace NoteMapper.Services.Logging
{
    public interface IErrorLoggingService
    {
        Task LogErrorMessageAsync(string message);

        Task LogExceptionAsync(Exception ex);
    }
}
