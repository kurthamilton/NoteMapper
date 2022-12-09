using System.Data;
using System.Data.SqlClient;
using NoteMapper.Core;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Repositories.Users
{
    public class UserSqlRepository : SqlRepositoryBase<User>, IUserRepository
    {
        public UserSqlRepository(SqlRepositorySettings settings)
            : base(settings)
        {
        }

        protected override string TableName => "Users";

        public Task<User?> CreateAsync(User user)
        {
            string sql = $"INSERT INTO {TableName} (CreatedUtc, Email) " +
                         "VALUES (@CreatedUtc, @Email)" +
                         "SELECT TOP 1 UserId, CreatedUtc, Email " +
                         $"FROM {TableName} " +
                         $"WHERE UserId = SCOPE_IDENTITY()";

            return ReadSingleAsync(sql, new[]
            {
                GetParameter("@CreatedUtc", user.CreatedUtc, SqlDbType.DateTime),
                GetParameter("@Email", user.Email, SqlDbType.NVarChar)
            });
        }

        public Task<ServiceResult> DeleteAsync(Guid userId)
        {
            string sql = $"DELETE {TableName} " +
                         "WHERE UserId = @UserId";

            return ExecuteQueryAsync(sql, new[]
            {
                GetParameter("@UserId", userId, SqlDbType.UniqueIdentifier)
            });
        }

        public Task<User?> FindAsync(Guid userId)
        {
            string sql = "SELECT TOP 1 UserId, CreatedUtc, Email " +
                         $"FROM {TableName} " +
                         $"WHERE UserId = @UserId";

            return ReadSingleAsync(sql, new[]
            {
                GetParameter("@UserId", userId, SqlDbType.UniqueIdentifier)
            });
        }

        public Task<User?> FindByEmailAsync(string email)
        {
            string sql = "SELECT TOP 1 UserId, CreatedUtc, Email " +
                         $"FROM {TableName} " +
                         $"WHERE Email = email";

            return ReadSingleAsync(sql, new[]
            {
                GetParameter("@Email", email, SqlDbType.NVarChar)
            });
        }

        protected override User Map(SqlDataReader reader)
        {
            return new User(reader.GetGuid(0),
                reader.GetDateTime(1),
                reader.GetString(2));
        }
    }
}
