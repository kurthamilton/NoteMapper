﻿using System.Data;
using System.Data.SqlClient;
using NoteMapper.Core;
using NoteMapper.Data.Core.Errors;
using NoteMapper.Data.Sql.Repositories;

namespace NoteMapper.Data.Sql
{
    public abstract class SqlRepositoryBase<T> where T : class
    {
        private readonly IApplicationErrorRepository _errorRepository;
        private readonly SqlRepositorySettings _settings;

        protected SqlRepositoryBase(SqlRepositorySettings settings, 
            IApplicationErrorRepository errorRepository)
        {
            _errorRepository = errorRepository;
            _settings = settings;
        }

        protected abstract IReadOnlyCollection<string> SelectColumns { get; }

        protected string SelectColumnSql => string.Join(", ", SelectColumns.Select(x => $"{TableName}.{x}"));

        protected abstract string TableName { get; }

        protected async Task<ServiceResult> ExecuteQueryAsync(string sql, IEnumerable<SqlParameter> parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = GetCommand(conn, sql, parameters))
                {
                    cmd.CommandType = CommandType.Text;

                    await conn.OpenAsync();

                    try
                    {
                        await cmd.ExecuteNonQueryAsync();
                        return ServiceResult.Successful();
                    }
                    catch (Exception ex)
                    {
                        await LogExceptionAsync(ex, cmd);
                        return ServiceResult.Failure("");
                    }
                }
            }
        }        

        protected SqlParameter GetParameter(string name, object? value, SqlDbType type)
        {
            return SqlUtils.GetParameter(name, value, type);
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
                        await LogExceptionAsync(ex, cmd);
                        return default;
                    }                    
                }
            }
        }        

        private Task LogExceptionAsync(Exception ex, SqlCommand cmd)
        {
            if (!_settings.LogErrors)
            {
                return Task.CompletedTask;
            }

            ApplicationError error = new(_settings.CurrentEnvironment, ex);
            error.AddProperty("Command.Sql", cmd.CommandText);

            foreach (SqlParameter parameter in cmd.Parameters)
            {
                error.AddProperty($"Command.Parameters.{parameter.ParameterName}", parameter.Value.ToString());
            }

            return _errorRepository.CreateAsync(error);
        }

        private SqlCommand GetCommand(SqlConnection conn, string sql, IEnumerable<SqlParameter> parameters)
        {
            SqlCommand cmd = new(sql, conn);
            cmd.Parameters.AddRange(parameters.ToArray());
            return cmd;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_settings.ConnectionString);
        }
    }
}