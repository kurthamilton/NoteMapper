﻿using System.Data;
using System.Data.SqlClient;
using NoteMapper.Core;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Repositories.Users
{
    public class UserActivationSqlRepository : SqlRepositoryBase<UserActivation>, IUserActivationRepository
    {
        public UserActivationSqlRepository(SqlRepositorySettings settings)
            : base(settings)
        {
        }

        protected override string TableName => "UserActivations";

        public Task<UserActivation?> CreateAsync(UserActivation userActivation)
        {
            string sql = $"INSERT INTO {TableName} (UserId, CreatedUtc, ExpiresUtc, Code) " +
                         "VALUES (@UserId, @CreatedUtc, @ExpiresUtc, @Code) " +
                         "SELECT TOP 1 UserId, CreatedUtc, ExpiresUtc, Code " +
                         $"FROM {TableName} " +
                         $"WHERE UserId = @UserId AND CreatedUtc = @CreatedUtc";

            return ReadSingleAsync(sql, new[]
            {
                GetParameter("@UserId", userActivation.UserId, SqlDbType.UniqueIdentifier),
                GetParameter("@CreatedUtc", userActivation.CreatedUtc, SqlDbType.DateTime),
                GetParameter("@ExpiresUtc", userActivation.ExpiresUtc, SqlDbType.DateTime),
                GetParameter("@Code", userActivation.Code, SqlDbType.NVarChar)
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

        public Task<UserActivation?> FindAsync(Guid userId, string code)
        {
            string sql = "SELECT TOP 1 UserId, CreatedUtc, ExpiresUtc, Code " +
                         $"FROM {TableName} " +
                         $"WHERE UserId = @UserId AND Code = @Code ";
            
            return ReadSingleAsync(sql, new[]
            {
                GetParameter("@UserId", userId, SqlDbType.UniqueIdentifier),
                GetParameter("@Code", code, SqlDbType.NVarChar)
            });
        }

        public Task<IReadOnlyCollection<UserActivation>> GetAllAsync(Guid userId)
        {
            string sql = "SELECT UserId, CreatedUtc, ExpiresUtc, Code " +
                         $"FROM {TableName} " +
                         $"WHERE UserId = @UserId";

            return ReadAsync(sql, new[]
            {
                GetParameter("@UserId", userId, SqlDbType.UniqueIdentifier)
            });
        }

        protected override UserActivation Map(SqlDataReader reader)
        {
            return new UserActivation(
                reader.GetGuid(0),
                reader.GetDateTime(1),
                reader.GetDateTime(2),
                reader.GetString(3));
        }
    }
}
