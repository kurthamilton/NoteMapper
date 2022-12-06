using NoteMapper.Core;

namespace NoteMapper.Data.Core.Users
{
    public interface IUserActivationRepository
    {
        Task<ServiceResult> CreateAsync(UserActivation activation);

        Task<ServiceResult> DeleteAllAsync(Guid userId);

        Task<UserActivation?> FindAsync(Guid userId, string code);

        Task<IReadOnlyCollection<UserActivation>> GetAllAsync(Guid userId);
    }
}
