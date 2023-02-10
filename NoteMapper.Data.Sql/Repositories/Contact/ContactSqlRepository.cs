﻿using System.Data;
using System.Data.SqlClient;
using NoteMapper.Core;
using NoteMapper.Data.Core.Contact;
using NoteMapper.Data.Core.Errors;

namespace NoteMapper.Data.Sql.Repositories.Contact
{
    public class ContactSqlRepository : SqlRepositoryBase<ContactRequest>, IContactRepository
    {
        public ContactSqlRepository(SqlRepositorySettings settings,
            IApplicationErrorRepository errorRepository)
            : base(settings, errorRepository)
        {
        }

        protected override string TableName => "ContactRequests";

        public Task<ServiceResult> CreateAsync(ContactRequest request)
        {
            string sql = $"INSERT INTO {TableName} (CreatedUtc, Email, Message) " +
                         "VALUES (@CreatedUtc, @Email, @Message) ";

            return ExecuteQueryAsync(sql, new[]
            {
                GetParameter("@CreatedUtc", request.CreatedUtc, SqlDbType.DateTime),
                GetParameter("@Email", request.Email, SqlDbType.NVarChar),
                GetParameter("@Message", request.Message, SqlDbType.NVarChar)
            });
        }

        protected override ContactRequest Map(SqlDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}