namespace NoteMapper.Data.Core.Errors
{
    public class ApplicationError
    {
        public ApplicationError(string message)
        {
            CreatedUtc = DateTime.UtcNow;
            Message = message;
            Properties = new Dictionary<string, string>();
        }

        public ApplicationError(Exception ex) 
            : this(ex.Message)
        {            
            Type = ex.GetType().Name;

            AddProperty("Exception.StackTrace", ex.StackTrace);
        }

        public ApplicationError(Exception ex, string url)
            : this(ex)
        {
            AddProperty("Url", url);
        }

        public DateTime CreatedUtc { get; }

        public string Message { get; }

        public Dictionary<string, string> Properties { get; } = new();

        public string? Type { get; }

        public ApplicationError AddProperty(string key, string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return this;
            }

            Properties.Add(key, value);
            return this;
        }
    }
}
