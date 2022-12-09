namespace NoteMapper.Data.Cosmos
{
    public class AzureCosmosRepositorySettings
    {
        public string ApplicationName { get; set; } = "";

        public string ConnectionString { get; set; } = "";

        public string DatabaseId { get; set; } = "";

        public string DefaultUserId { get; set; } = "";
    }
}
