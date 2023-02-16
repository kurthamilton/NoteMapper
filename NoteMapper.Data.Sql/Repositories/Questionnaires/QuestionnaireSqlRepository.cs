using System.Data;
using System.Data.SqlClient;
using NoteMapper.Data.Core.Errors;
using NoteMapper.Data.Core.Questionnaires;
using NoteMapper.Data.Sql.Extensions;

namespace NoteMapper.Data.Sql.Repositories.Questionnaires
{
    public class QuestionnaireSqlRepository : SqlRepositoryBase<Questionnaire>, IQuestionnaireRepository
    {
        protected override IReadOnlyCollection<string> SelectColumns => new[]
        {
            "QuestionnaireId", "Name", "ExpiresUtc"
        };

        protected override string TableName => "Questionnaires";

        public QuestionnaireSqlRepository(SqlRepositorySettings settings, 
            IApplicationErrorRepository applicationErrorRepository)
            : base(settings, applicationErrorRepository)
        {
        }

        public Task<Questionnaire?> FindAsync(Guid questionnaireId)
        {
            string sql = $"SELECT {SelectColumnSql} " +
                         $"FROM {TableName} " +
                         $"WHERE QuestionnaireId = @QuestionnaireId ";

            return ReadSingleAsync(sql, new[]
            {
                GetParameter("@QuestionnaireId", questionnaireId, SqlDbType.UniqueIdentifier)
            });
        }

        public Task<Questionnaire?> GetCurrentAsync()
        {
            string sql = $"SELECT TOP 1 {SelectColumnSql} " +
                         $"FROM {TableName} " +
                         $"WHERE Active = 1 AND (ExpiresUtc IS NULL OR ExpiresUtc > GETUTCDATE()) " +
                         $"ORDER BY CreatedUtc DESC ";

            return ReadSingleAsync(sql, Array.Empty<SqlParameter>());
        }

        protected override Questionnaire Map(SqlDataReader reader)
        {
            return new Questionnaire(
                reader.GetGuid(0),
                reader.GetString(1),
                reader.GetDateTimeOrNull(2));
        }
    }
}
