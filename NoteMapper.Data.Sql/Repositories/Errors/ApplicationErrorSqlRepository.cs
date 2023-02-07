using System.Data;
using System.Data.SqlClient;
using NoteMapper.Data.Core.Errors;

namespace NoteMapper.Data.Sql.Repositories.Errors
{
    public class ApplicationErrorSqlRepository : IApplicationErrorRepository
    {
        private readonly SqlRepositorySettings _settings;

        public ApplicationErrorSqlRepository(SqlRepositorySettings settings)
        {
            _settings = settings;
        }

        private string TableName => "ApplicationErrors";

        public async Task CreateAsync(ApplicationError error)
        {
            string sql = $"INSERT INTO {TableName} (CreatedUtc, Message, Type) " +
                         "VALUES (@CreatedUtc, @Message, @Type)" +
                         "SELECT TOP 1 ApplicationErrorId " +
                         $"FROM {TableName} " +
                         $"WHERE CreatedUtc = @CreatedUtc AND Message = @Message AND Type = @Type";

            Guid? applicationErrorId = null; 

            using (SqlConnection conn = new(_settings.ConnectionString))
            {
                using (SqlCommand cmd = new(sql, conn))
                {
                    cmd.Parameters.Add(SqlUtils.GetParameter("@CreatedUtc", error.CreatedUtc, SqlDbType.DateTime));
                    cmd.Parameters.Add(SqlUtils.GetParameter("@Message", error.Message, SqlDbType.NVarChar));
                    cmd.Parameters.Add(SqlUtils.GetParameter("@Type", error.Type, SqlDbType.NVarChar));

                    await conn.OpenAsync();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        applicationErrorId = reader.GetGuid(0);                                                
                    }                    
                }

                if (applicationErrorId == null) 
                {
                    return;
                }                
            }

            foreach (string key in error.Properties.Keys)
            {
                if (error.Properties[key] == null)
                {
                    continue;
                }

                sql = "INSERT INTO ApplicationErrorProperties (ApplicationErrorId, Name, Value) " +
                      "VALUES (@ApplicationErrorId, @Name, @Value) ";

                using (SqlConnection conn = new SqlConnection(_settings.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        await conn.OpenAsync();

                        cmd.Parameters.Add(SqlUtils.GetParameter("@ApplicationErrorId", applicationErrorId, SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(SqlUtils.GetParameter("@Name", key, SqlDbType.NVarChar));
                        cmd.Parameters.Add(SqlUtils.GetParameter("@Value", error.Properties[key], SqlDbType.NVarChar));

                        await cmd.ExecuteNonQueryAsync();
                    }
                }                
            }
        }
    }
}
