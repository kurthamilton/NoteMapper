using NoteMapper.Core;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Repositories.Users
{
    public class UserLoginTokenSqlRepository : SqlRepositoryBase<UserLoginToken>, IUserLoginTokenRepository
    {
        public UserLoginTokenSqlRepository(NoteMapperContext context)
            : base(context)
        {
        }

        public Task<ServiceResult> DeleteAllAsync(Guid userId)
        {
            return DeleteWhereAsync(x => x.UserId == userId);
        }

        public Task<UserLoginToken?> FindAsync(Guid userId, string token)
        {
            return FirstOrDefaultAsync(x => x.UserId == userId && x.Token == token);
        }
    }
}
