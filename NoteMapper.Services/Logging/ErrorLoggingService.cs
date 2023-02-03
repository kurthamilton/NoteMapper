using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteMapper.Data.Core.Errors;

namespace NoteMapper.Services.Logging
{
    public class ErrorLoggingService : IErrorLoggingService
    {
        private readonly IApplicationErrorRepository _applicationErrorRepository;

        public ErrorLoggingService(IApplicationErrorRepository applicationErrorRepository)
        {
            _applicationErrorRepository = applicationErrorRepository;
        }

        public Task LogErrorMessageAsync(string message)
        {
            ApplicationError error = new ApplicationError(message);
            return _applicationErrorRepository.CreateAsync(error);
        }

        public Task LogExceptionAsync(Exception ex)
        {
            ApplicationError error = new ApplicationError(ex.Message,
                ex.GetType().Name);
            return _applicationErrorRepository.CreateAsync(error);
        }
    }
}
