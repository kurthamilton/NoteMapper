using NoteMapper.Core;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Repositories.Users
{
    public class UserPasswordSqlRepository : SqlRepositoryBase<UserPassword>, IUserPasswordRepository
    {
        public UserPasswordSqlRepository(NoteMapperContext context)
            : base(context)
        {
        }

        public Task<UserPassword?> FindAsync(Guid userId)
        {
            return FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
