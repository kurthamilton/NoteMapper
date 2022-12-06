using NoteMapper.Core;

namespace NoteMapper.Data.Core.Users
{
    public interface IUserLoginTokenRepository
    {
        Task<ServiceResult> CreateAsync(UserLoginToken token);

        Task<ServiceResult> DeleteAllAsync(Guid userId);

        Task<UserLoginToken?> FindAsync(Guid userId, string token);
    }
}
