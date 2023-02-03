namespace NoteMapper.Data.Core.Errors
{
    public class ApplicationError
    {
        public ApplicationError(string message)
        {
            CreatedUtc = DateTime.UtcNow;
            Message = message;
        }

        public ApplicationError(string message, string type) 
            : this(message)
        {            
            Type = type;
        }

        public DateTime CreatedUtc { get; }

        public string Message { get; }

        public string? Type { get; }
    }
}
