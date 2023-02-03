using System.Data;
using System.Data.SqlClient;
using NoteMapper.Data.Core.Errors;

namespace NoteMapper.Data.Sql.Repositories.Errors
{
    public class ApplicationErrorSqlRepository : SqlRepositoryBase<ApplicationError>, 
        IApplicationErrorRepository
    {
        public ApplicationErrorSqlRepository(SqlRepositorySettings settings)
            : base(settings)
        {
        }

        protected override string TableName => "ApplicationErrors";

        public Task CreateAsync(ApplicationError error)
        {
            string sql = $"INSERT INTO {TableName} (CreatedUtc, Message, Type) " +
                         "VALUES (@CreatedUtc, @Message, @Type)";
            
            return ExecuteQueryAsync(sql, new[]
            {
                GetParameter("@CreatedUtc", error.CreatedUtc, SqlDbType.DateTime),
                GetParameter("@Message", error.Message, SqlDbType.NVarChar),
                GetParameter("@Type", error.Type, SqlDbType.NVarChar)
            });
        }

        protected override ApplicationError Map(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
