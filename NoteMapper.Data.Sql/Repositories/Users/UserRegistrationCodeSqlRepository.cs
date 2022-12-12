using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Data.Sql.Repositories.Users
{
    public class UserRegistrationCodeSqlRepository : SqlRepositoryBase<UserRegistrationCode>, IUserRegistrationCodeRepository
    {
        public UserRegistrationCodeSqlRepository(SqlRepositorySettings settings) 
            : base(settings)
        {
        }

        protected override string TableName => "UserRegistrationCodes";

        public Task<UserRegistrationCode?> CreateAsync(UserRegistrationCode userRegistrationCode)
        {
            string sql = $"INSERT INTO {TableName} (UserId, RegistrationCodeId, CreatedUtc) " +
                         "VALUES (@UserId, @RegistrationCodeId, @CreatedUtc) " +
                         "SELECT TOP 1 UserId, RegistrationCodeId, CreatedUtc " +
                         $"FROM {TableName} " +
                         "WHERE UserId = @UserId AND RegistrationCodeId = @RegistrationCodeId";

            return ReadSingleAsync(sql, new[]
            {
                GetParameter("@UserId", userRegistrationCode.UserId, SqlDbType.UniqueIdentifier),
                GetParameter("@RegistrationCodeId", userRegistrationCode.RegistrationCodeId, SqlDbType.UniqueIdentifier),
                GetParameter("@CreatedUtc", userRegistrationCode.CreatedUtc, SqlDbType.DateTime)
            });
        }

        protected override UserRegistrationCode Map(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
