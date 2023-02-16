namespace NoteMapper.Data.Core.Questionnaires
{
    public interface IQuestionnaireRepository
    {
        Task<Questionnaire?> FindAsync(Guid questionnaireId);

        Task<Questionnaire?> GetCurrentAsync();        
    }
}
