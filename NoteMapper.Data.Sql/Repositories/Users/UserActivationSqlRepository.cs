using NoteMapper.Core;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Repositories.Users
{
    public class UserActivationSqlRepository : SqlRepositoryBase<UserActivation>, IUserActivationRepository
    {
        public UserActivationSqlRepository(NoteMapperContext context)
            : base(context)
        {
        }

        public Task<ServiceResult> DeleteAllAsync(Guid userId)
        {
            return DeleteWhereAsync(x => x.UserId == userId);
        }

        public Task<UserActivation?> FindAsync(Guid userId, string code)
        {
            return FirstOrDefaultAsync(x => x.UserId == userId && x.Code == code);
        }

        public Task<IReadOnlyCollection<UserActivation>> GetAllAsync(Guid userId)
        {
            return Select(x => x.UserId == userId);
        }
    }
}
