using System.Data;
using System.Data.SqlClient;
using NoteMapper.Data.Core.Errors;
using NoteMapper.Data.Core.Questionnaires;
using NoteMapper.Data.Sql.Extensions;

namespace NoteMapper.Data.Sql.Repositories.Questionnaires
{
    public class QuestionnaireQuestionSqlRepository : SqlRepositoryBase<QuestionnaireQuestion>, IQuestionnaireQuestionRepository
    {
        public QuestionnaireQuestionSqlRepository(SqlRepositorySettings settings, 
            IApplicationErrorRepository applicationErrorRepository)
            : base(settings, applicationErrorRepository)
        {
        }

        protected override IReadOnlyCollection<string> SelectColumns => new[]
        {
            "QuestionId", "QuestionnaireId", "QuestionText", "QuestionTypeId", "Required", "Range"
        };

        protected override string TableName => "QuestionnaireQuestions";

        public Task<IReadOnlyCollection<QuestionnaireQuestion>> GetQuestionsAsync(Guid questionnaireId)
        {
            string sql = $"SELECT {SelectColumnSql} " +
                         $"FROM {TableName} " +
                         $"WHERE QuestionnaireId = @QuestionnaireId " +
                         $"ORDER BY DisplayOrder ";

            return ReadAsync(sql, new[]
            {
                GetParameter("@QuestionnaireId", questionnaireId, SqlDbType.UniqueIdentifier)
            });
        }

        protected override QuestionnaireQuestion Map(SqlDataReader reader)
        {
            return new QuestionnaireQuestion(
                reader.GetGuid(0),
                reader.GetGuid(1),
                reader.GetString(2),
                (QuestionType)reader.GetInt32(3),
                reader.GetBoolean(4),
                reader.GetStringOrNull(5));
        }
    }
}
