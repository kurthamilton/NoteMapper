namespace NoteMapper.Data.Core.Questionnaires
{
    public interface IQuestionnaireQuestionRepository
    {
        public Task<IReadOnlyCollection<QuestionnaireQuestion>> GetQuestionsAsync(Guid questionnaireId);
    }
}
