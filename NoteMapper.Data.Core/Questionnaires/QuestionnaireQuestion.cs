namespace NoteMapper.Data.Core.Questionnaires
{
    public class QuestionnaireQuestion
    {
        public QuestionnaireQuestion(Guid questionId, Guid questionnaireId, string questionText,
            QuestionType type, bool required, string? range)
        {
            QuestionId = questionId;
            QuestionnaireId = questionnaireId;
            QuestionText = questionText;
            Range = range;
            Required = required;
            Type = type;
        }

        public Guid QuestionId { get; }

        public Guid QuestionnaireId { get; }

        public string QuestionText { get; }

        public string? Range { get; }

        public bool Required { get; }

        public QuestionType Type { get; }
    }
}
