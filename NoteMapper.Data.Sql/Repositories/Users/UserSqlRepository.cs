using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Repositories.Users
{
    public class UserSqlRepository : SqlRepositoryBase<User>, IUserRepository
    {
        public UserSqlRepository(NoteMapperContext context)
            : base(context)
        {
        }

        public Task DeleteAsync(Guid userId)
        {
            return DeleteWhereAsync(x => x.UserId == userId);
        }

        public Task<User?> FindAsync(Guid userId)
        {
            return FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public Task<User?> FindByEmailAsync(string email)
        {
            return FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
