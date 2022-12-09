using System.Diagnostics.Metrics;
using Microsoft.Azure.Cosmos;
using NoteMapper.Core;
using NoteMapper.Data.Core.Instruments;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Cosmos.Repositories
{
    public class UserInstrumentAzureCosmosRepository : AzureCosmosRepositoryBase<UserInstruments>, IUserInstrumentRepository
    {
        public UserInstrumentAzureCosmosRepository(AzureCosmosRepositorySettings settings) 
            : base(settings)
        {
        }

        protected override string ContainerId => "user-instruments";

        public async Task<ServiceResult> CreateUserInstrumentAsync(Guid userId, UserInstrument instrument)
        {
            using (CosmosClient client = CreateClient())
            {
                Container container = GetContainer(client);
                UserInstruments? entry = await FindAsync(container, userId.ToString());
                if (entry == null)
                {
                    entry = new UserInstruments
                    {
                        Id = userId.ToString()
                    };

                    ServiceResult createResult = await CreateAsync(container, userId.ToString(), entry);
                    if (!createResult.Success)
                    {
                        return createResult;
                    }
                }

                entry.Instruments.Add(instrument);
                return await UpdateAsync(container, userId.ToString(), entry);
            }            
        }

        public async Task<ServiceResult> DeleteUserInstrumentAsync(Guid userId, UserInstrument instrument)
        {
            using (CosmosClient client = CreateClient())
            {
                Container container = GetContainer(client);
                UserInstruments? entry = await FindAsync(container, userId.ToString());
                if (entry == null)
                {
                    return ServiceResult.Successful();
                }

                for (int i = entry.Instruments.Count - 1; i >= 0; i--)
                {
                    if (entry.Instruments[i].Name == instrument.Name)
                    {
                        entry.Instruments.RemoveAt(i);
                    }
                }

                return await UpdateAsync(container, userId.ToString(), entry);
            }
        }

        public async Task<IReadOnlyCollection<UserInstrument>> GetDefaultInstrumentsAsync()
        {
            using (CosmosClient client = CreateClient())
            {
                Container container = GetContainer(client);

                UserInstruments? entry = await FindAsync(container, DefaultUserId);
                if (entry == null)
                {
                    return Array.Empty<UserInstrument>();
                }

                return entry.Instruments.ToArray();
            }
        }

        public async Task<IReadOnlyCollection<UserInstrument>> GetUserInstrumentsAsync(Guid userId)
        {
            using (CosmosClient client = CreateClient())
            {
                Container container = GetContainer(client);

                UserInstruments? entry = await FindAsync(container, userId.ToString());
                if (entry == null)
                {
                    return Array.Empty<UserInstrument>();
                }

                return entry.Instruments.ToArray();
            }            
        }

        public async Task<ServiceResult> UpdateUserInstrumentAsync(Guid userId, UserInstrument instrument)
        {
            using (CosmosClient client = CreateClient())
            {
                Container container = GetContainer(client);
                UserInstruments? entry = await FindAsync(container, userId.ToString());
                if (entry == null)
                {
                    return ServiceResult.Successful();
                }

                for (int i = 0; i < entry.Instruments.Count; i++)
                {
                    if (entry.Instruments[i].Name == instrument.Name)
                    {
                        entry.Instruments[i] = instrument;
                    }
                }

                return await UpdateAsync(container, userId.ToString(), entry);
            }
        }
    }
}
