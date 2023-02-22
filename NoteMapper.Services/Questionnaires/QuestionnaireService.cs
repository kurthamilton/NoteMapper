using NoteMapper.Core;
using NoteMapper.Data.Core.Questionnaires;

namespace NoteMapper.Services.Questionnaires
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly IQuestionnaireRepository _questionnaireRepository;

        public QuestionnaireService(IQuestionnaireRepository questionnaireRepository)
        {
            _questionnaireRepository = questionnaireRepository;
        }

        public async Task<ServiceResult> DeleteQuestionnaireAsync(Guid questionnaireId)
        {
            ServiceResult result = await _questionnaireRepository.DeleteAsync(questionnaireId);
            return result.Success
                ? ServiceResult.Successful("Questionnaire deleted")
                : ServiceResult.Failure("Questionnaire could not be deleted");
        }

        public Task<IReadOnlyCollection<Questionnaire>> GetQuestionnairesAsync()
        {
            return _questionnaireRepository.GetAllAsync();
        }
    }
}
