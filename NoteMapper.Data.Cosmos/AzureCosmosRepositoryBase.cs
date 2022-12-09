using System.Net;
using Microsoft.Azure.Cosmos;
using NoteMapper.Core;

namespace NoteMapper.Data.Cosmos
{
    public abstract class AzureCosmosRepositoryBase<T>
    {
        private readonly AzureCosmosRepositorySettings _settings;

        protected AzureCosmosRepositoryBase(AzureCosmosRepositorySettings settings)
        {
            _settings = settings;
            DefaultUserId = _settings.DefaultUserId;
        }

        protected abstract string ContainerId { get; }

        protected string DefaultUserId { get; private set; }

        protected async Task<ServiceResult> CreateAsync(Container container, string id, T entity)
        {
            ItemResponse<T> response = await container.CreateItemAsync(entity, new PartitionKey(id));
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return ServiceResult.Failure("Error creating entity");
            }

            return ServiceResult.Successful();
        }

        protected CosmosClient CreateClient()
        {
            CosmosClientOptions options = new CosmosClientOptions()
            {
                ApplicationName = _settings.ApplicationName
            };

            return new CosmosClient(_settings.ConnectionString, options);
        }

        protected async Task<T?> FindAsync(Container container, string id)
        {
            try
            {
                ItemResponse<T> response = await container.ReadItemAsync<T>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch
            {
                return default;
            }
        }

        protected Container GetContainer(CosmosClient client)
        {
            return client.GetContainer(_settings.DatabaseId, ContainerId);
        }

        protected async Task<ServiceResult> UpdateAsync(Container container, string id, T entity)
        {
            ItemResponse<T> response = await container.UpsertItemAsync<T>(entity, new PartitionKey(id));
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return ServiceResult.Failure("Error updating entity");
            }

            return ServiceResult.Successful();
        }                
    }
}
