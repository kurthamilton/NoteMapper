namespace NoteMapper.Data.Core.Questionnaires
{
    public class Questionnaire
    {
        public Questionnaire(Guid questionnaireId, string name, DateTime? expiresUtc)
        {
            ExpiresUtc = expiresUtc;
            Name = name;
            QuestionnaireId = questionnaireId;
        }

        public DateTime? ExpiresUtc { get; set; }

        public string Name { get; }

        public Guid QuestionnaireId { get; }
    }
}
