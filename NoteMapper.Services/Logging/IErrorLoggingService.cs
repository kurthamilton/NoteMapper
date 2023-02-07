namespace NoteMapper.Services.Logging
{
    public interface IErrorLoggingService
    {
        Task LogErrorMessageAsync(string message);

        Task LogErrorMessageAsync(string message, IDictionary<string, string> data);

        Task LogExceptionAsync(Exception ex);

        Task LogExceptionAsync(Exception ex, string url);
    }
}
