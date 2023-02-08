using NoteMapper.Data.Core.Errors;

namespace NoteMapper.Services.Logging
{
    public class ErrorLoggingService : IErrorLoggingService
    {
        private readonly IApplicationErrorRepository _applicationErrorRepository;
        private readonly ErrorLoggingServiceSettings _settings;

        public ErrorLoggingService(IApplicationErrorRepository applicationErrorRepository, 
            ErrorLoggingServiceSettings settings)
        {
            _applicationErrorRepository = applicationErrorRepository;
            _settings = settings;
        }

        public Task LogErrorMessageAsync(string message)
        {
            return LogErrorMessageAsync(message, new Dictionary<string, string>());
        }

        public Task LogErrorMessageAsync(string message, IDictionary<string, string> data)
        {
            ApplicationError error = new(message);
            foreach (string key in data.Keys)
            {
                error.AddProperty(key, data[key]);
            }
            return LogExceptionAsync(error);
        }

        public Task LogExceptionAsync(Exception ex)
        {
            ApplicationError error = new(ex);
            return LogExceptionAsync(error);
        }

        public Task LogExceptionAsync(Exception ex, string url)
        {
            ApplicationError error = new(ex, url);
            return LogExceptionAsync(error);
        }

        private Task LogExceptionAsync(ApplicationError error)
        {
            if (!_settings.Enabled)
            {
                return Task.CompletedTask;
            }

            return _applicationErrorRepository.CreateAsync(error);
        }
    }
}
