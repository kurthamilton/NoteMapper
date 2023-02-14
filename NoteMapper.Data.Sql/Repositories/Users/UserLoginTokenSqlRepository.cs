using System.Data;
using System.Data.SqlClient;
using NoteMapper.Core;
using NoteMapper.Data.Core.Errors;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Repositories.Users
{
    public class UserLoginTokenSqlRepository : SqlRepositoryBase<UserLoginToken>, IUserLoginTokenRepository
    {
        public UserLoginTokenSqlRepository(SqlRepositorySettings settings,
            IApplicationErrorRepository errorRepository)
            : base(settings, errorRepository)
        {
        }

        protected override IReadOnlyCollection<string> SelectColumns => new[]
        {
            "UserId", "CreatedUtc", "ExpiresUtc", "Token"
        };

        protected override string TableName => "UserLoginTokens";

        public Task<UserLoginToken?> CreateAsync(UserLoginToken token)
        {
            string sql = $"INSERT INTO {TableName} (UserId, CreatedUtc, ExpiresUtc, Token) " +
                         $"VALUES (@UserId, @CreatedUtc, @ExpiresUtc, @Token) " +
                         $"SELECT TOP 1 {SelectColumnSql} " +
                         $"FROM {TableName} " +
                         "WHERE UserId = @UserId AND CreatedUtc = @CreatedUtc";

            return ReadSingleAsync(sql, new[]
            {
                GetParameter("@UserId", token.UserId, SqlDbType.UniqueIdentifier),
                GetParameter("@CreatedUtc", token.CreatedUtc, SqlDbType.DateTime),
                GetParameter("@ExpiresUtc", token.ExpiresUtc, SqlDbType.DateTime),
                GetParameter("@Token", token.Token, SqlDbType.NVarChar)
            });
        }

        public Task<ServiceResult> DeleteAllAsync(Guid userId)
        {
            string sql = $"DELETE {TableName} " +
                         $"WHERE UserId = @UserId";

            return ExecuteQueryAsync(sql, new[]
            {
                GetParameter("@UserId", userId, SqlDbType.UniqueIdentifier)
            });
        }

        public Task<UserLoginToken?> FindAsync(Guid userId, string token)
        {
            string sql = $"SELECT TOP 1 {SelectColumnSql} " +
                         $"FROM {TableName} " +
                         $"WHERE UserId = @UserId AND Token = @Token";

            return ReadSingleAsync(sql, new[]
            {
                GetParameter("@UserId", userId, SqlDbType.UniqueIdentifier),
                GetParameter("@Token", token, SqlDbType.NVarChar)
            });
        }

        protected override UserLoginToken Map(SqlDataReader reader)
        {
            return new UserLoginToken(reader.GetGuid(0),
                reader.GetDateTime(1),
                reader.GetDateTime(2),
                reader.GetString(3));
        }
    }
}
