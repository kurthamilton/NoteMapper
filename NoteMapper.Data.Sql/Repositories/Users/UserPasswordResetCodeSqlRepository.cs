using System.Data;
using System.Data.SqlClient;
using NoteMapper.Core;
using NoteMapper.Data.Core.Errors;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Repositories.Users
{
    public class UserPasswordResetCodeSqlRepository : SqlRepositoryBase<UserPasswordResetCode>, IUserPasswordResetCodeRepository
    {
        public UserPasswordResetCodeSqlRepository(SqlRepositorySettings settings,
            IApplicationErrorRepository errorRepository) 
            : base(settings, errorRepository)
        {
        }

        protected override string TableName => "UserPasswordResetCodes";

        public Task<UserPasswordResetCode?> CreateAsync(UserPasswordResetCode resetCode)
        {
            string sql = $"INSERT INTO {TableName} (UserId, CreatedUtc, ExpiresUtc, Code) " +
                         "VALUES (@UserId, @CreatedUtc, @ExpiresUtc, @Code) " +
                         "SELECT UserId, CreatedUtc, ExpiresUtc, Code " +
                         $"FROM {TableName} " +
                         $"WHERE UserId = @UserId AND CreatedUtc = @CreatedUtc";

            return ReadSingleAsync(sql, new[]
            {
                GetParameter("@UserId", resetCode.UserId, SqlDbType.UniqueIdentifier),
                GetParameter("@CreatedUtc", resetCode.CreatedUtc, SqlDbType.DateTime),
                GetParameter("@ExpiresUtc", resetCode.ExpiresUtc, SqlDbType.DateTime),
                GetParameter("@Code", resetCode.Code, SqlDbType.NVarChar)
            });
        }

        public Task<ServiceResult> DeleteAllAsync(Guid userId)
        {
            string sql = $"DELETE {TableName} " +
                         "WHERE UserId = @UserId";

            return ExecuteQueryAsync(sql, new[]
            {
                GetParameter("@UserId", userId, SqlDbType.UniqueIdentifier)
            });
        }

        public Task<UserPasswordResetCode?> FindAsync(Guid userId, string code)
        {
            string sql = "SELECT TOP 1 UserId, CreatedUtc, ExpiresUtc, Code " +
                         $"FROM {TableName} " +
                         "WHERE UserId = @UserId AND Code = @Code";

            return ReadSingleAsync(sql, new[]
            {
                GetParameter("@UserId", userId, SqlDbType.UniqueIdentifier),
                GetParameter("@Code", code, SqlDbType.NVarChar)
            });
        }

        protected override UserPasswordResetCode Map(SqlDataReader reader)
        {
            return new UserPasswordResetCode(reader.GetGuid(0),
                reader.GetDateTime(1),
                reader.GetDateTime(2),
                reader.GetString(3));
        }
    }
}
