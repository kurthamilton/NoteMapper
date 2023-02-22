﻿using System.Data;
using System.Data.SqlClient;
using NoteMapper.Core;
using NoteMapper.Data.Core.Errors;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Repositories.Users
{
    public class UserPreferenceSqlRepository : SqlRepositoryBase<UserPreference>, IUserPreferenceRepository
    {
        protected override IReadOnlyCollection<string> SelectColumns => new[]
        {
            "UserPreferenceTypeId", "Value"
        };

        protected override string TableName => "UserPreferences";

        public UserPreferenceSqlRepository(SqlRepositorySettings settings, 
            IApplicationErrorRepository applicationErrorRepository)
            : base(settings, applicationErrorRepository)
        {
        }

        public Task<IReadOnlyCollection<UserPreference>> GetAsync(Guid userId)
        {
            string sql = $"SELECT {SelectColumnSql} " +
                         $"FROM {TableName} " +
                         $"WHERE UserId = @UserId ";

            return ReadManyAsync(sql, new[]
            {
                GetParameter("@UserId", userId, SqlDbType.UniqueIdentifier)
            });
        }

        public Task<ServiceResult> UpdateAsync(Guid userId, IReadOnlyCollection<UserPreference> preferences)
        {
            string sql = "";
            List<SqlParameter> parameters = new()
            {
                GetParameter("@UserId", userId, SqlDbType.UniqueIdentifier)
            };

            for (int i = 0; i < preferences.Count; i++)
            {
                UserPreference preference = preferences.ElementAt(i);

                sql += "IF NOT EXISTS( " +
                       "    SELECT * " +
                       "    FROM UserPreferences " +
                       $"    WHERE UserId = @UserId AND UserPreferenceTypeId = @Type{i}) " +
                       "BEGIN " +
                       "    INSERT INTO UserPreferences (UserId, UserPreferenceTypeId, Value) " +
                       $"    VALUES (@UserId, @Type{i}, @Value{i}) " +
                       "END " +
                       "ELSE " +
                       "BEGIN " +
                       $"    UPDATE UserPreferences SET Value = @Value{i} " +
                       $"    WHERE UserId = @UserId AND UserPreferenceTypeId = @Type{i} " +
                       "END; ";

                parameters.AddRange(new[]
                {
                    GetParameter($"@Type{i}", (int)preference.Type, SqlDbType.Int),
                    GetParameter($"@Value{i}", preference.Value, SqlDbType.NVarChar)
                });
            }

            return ExecuteQueryAsync(sql, parameters);
        }

        protected override UserPreference Map(SqlDataReader reader)
        {
            return new UserPreference((UserPreferenceType)reader.GetInt32(0),
                reader.GetString(1));
        }
    }
}
