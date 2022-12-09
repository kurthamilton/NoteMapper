using System.Data;
using System.Data.SqlClient;
using NoteMapper.Core;
using NoteMapper.Data.Sql.Repositories;

namespace NoteMapper.Data.Sql
{
    public abstract class SqlRepositoryBase<T> where T : class
    {
        private readonly SqlRepositorySettings _settings;

        protected SqlRepositoryBase(SqlRepositorySettings settings)
        {
            _settings = settings;
        }

        protected abstract string TableName { get; }

        protected async Task<ServiceResult> ExecuteQueryAsync(string sql, IEnumerable<SqlParameter> parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = GetCommand(conn, sql, parameters))
                {
                    await conn.OpenAsync();

                    try
                    {
                        await cmd.ExecuteNonQueryAsync();
                        return ServiceResult.Successful();
                    }
                    catch
                    {
                        return ServiceResult.Failure("");
                    }
                }
            }
        }        

        protected SqlParameter GetParameter(string name, object? value, SqlDbType type)
        {
            SqlParameter parameter = new();
            parameter.ParameterName = name;

            if (value != null)
            {
                parameter.Value = value;
            }            
            else
            {
                parameter.Value = DBNull.Value;
            }
            return parameter;
        }

        protected abstract T Map(SqlDataReader reader);

        protected async Task<IReadOnlyCollection<T>> ReadAsync(string sql, IEnumerable<SqlParameter> parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = GetCommand(conn, sql, parameters))
                {
                    await conn.OpenAsync();

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    if (!await reader.ReadAsync())
                    {
                        return Array.Empty<T>();
                    }

                    List<T> entities = new();
                    do
                    {
                        T entity = Map(reader);
                        entities.Add(entity);
                    } while (await reader.ReadAsync());

                    return entities;
                }
            }
        }

        protected async Task<T?> ReadSingleAsync(string sql, IEnumerable<SqlParameter> parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = GetCommand(conn, sql, parameters))
                {
                    await conn.OpenAsync();

                    try
                    {
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        if (!await reader.ReadAsync())
                        {
                            return default;
                        }

                        return Map(reader);
                    }
                    catch (Exception ex)
                    {
                        return default;
                    }                    
                }
            }
        }        

        private SqlCommand GetCommand(SqlConnection conn, string sql, IEnumerable<SqlParameter> parameters)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddRange(parameters.ToArray());
            return cmd;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_settings.ConnectionString);
        }
    }
}