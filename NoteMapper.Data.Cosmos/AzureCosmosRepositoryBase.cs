using System.Net;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

        protected Task<ServiceResult> CreateAsync(Container container, string id, T entity)
        {
            return UpdateAsync(container, id, entity);
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
            try
            {
                object mapped = MapEntity(entity);
                ItemResponse<object> response = await container.UpsertItemAsync(mapped, new PartitionKey(id));
                return ServiceResult.Successful();
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure("Error updating entity");
            }                        
        }   
        
        private object MapEntity(T entity)
        {
            JsonSerializerSettings settings = new();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            string json = JsonConvert.SerializeObject(entity, settings);
            object result = JsonConvert.DeserializeObject(json);
            return result;
        }
    }
}
