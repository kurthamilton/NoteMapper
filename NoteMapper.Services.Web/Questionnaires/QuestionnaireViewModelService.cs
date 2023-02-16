using NoteMapper.Core;
using NoteMapper.Data.Core.Questionnaires;
using NoteMapper.Services.Web.ViewModels.Questionnaires;

namespace NoteMapper.Services.Web.Questionnaires
{
    public class QuestionnaireViewModelService : IQuestionnaireViewModelService
    {
        private readonly IQuestionnaireQuestionRepository _questionnaireQuestionRepository;
        private readonly IQuestionnaireRepository _questionnaireRepository;
        private readonly IUserQuestionResponseRepository _userQuestionResponseRepository;

        public QuestionnaireViewModelService(IQuestionnaireRepository questionnaireRepository,
            IQuestionnaireQuestionRepository questionnaireQuestionRepository,
            IUserQuestionResponseRepository userQuestionResponseRepository)
        {
            _questionnaireQuestionRepository = questionnaireQuestionRepository;
            _questionnaireRepository = questionnaireRepository;
            _userQuestionResponseRepository = userQuestionResponseRepository;
        }

        public async Task<QuestionnaireResponsesViewModel?> GetLatestQuestionnaireResponsesAsync(Guid userId)
        {
            Questionnaire? questionnaire = await _questionnaireRepository.GetCurrentAsync();
            if (questionnaire == null)
            {
                return null;
            }

            IReadOnlyCollection<QuestionnaireQuestion> questions = await _questionnaireQuestionRepository.GetQuestionsAsync(
                questionnaire.QuestionnaireId);

            IReadOnlyCollection<UserQuestionResponse> responses = await _userQuestionResponseRepository.GetAsync(
                userId, questionnaire.QuestionnaireId);

            List<QuestionnaireResponseViewModel> responseViewModels = new();

            foreach (QuestionnaireQuestion question in questions)
            {
                UserQuestionResponse? response = responses.FirstOrDefault(x => x.QuestionId == question.QuestionId);
                responseViewModels.Add(new QuestionnaireResponseViewModel(question, response));
            }

            return new QuestionnaireResponsesViewModel(questionnaire.QuestionnaireId,
                responseViewModels);
        }

        public async Task<ServiceResult> SaveResponsesAsync(Guid userId,
            QuestionnaireResponsesViewModel viewModel)
        {
            Questionnaire? questionnaire = await _questionnaireRepository.FindAsync(viewModel.QuestionnaireId);
            if (questionnaire == null)
            {
                return ServiceResult.Failure("Error saving responses");
            }

            IReadOnlyCollection<QuestionnaireQuestion> questions = await _questionnaireQuestionRepository.GetQuestionsAsync(
                questionnaire.QuestionnaireId);

            List<UserQuestionResponse> responses = new List<UserQuestionResponse>(await _userQuestionResponseRepository.GetAsync(
                userId, questionnaire.QuestionnaireId));

            foreach (QuestionnaireResponseViewModel responseViewModel in viewModel.Responses)
            {
                QuestionnaireQuestion? question = questions.FirstOrDefault(x => x.QuestionId == responseViewModel.QuestionId);
                if (question == null)
                {
                    continue;
                }

                UserQuestionResponse? response = responses.FirstOrDefault(x => x.QuestionId == question.QuestionId);
                if (response == null)
                {
                    response = new UserQuestionResponse(Guid.Empty, userId, question.QuestionId, responseViewModel.Value);
                    responses.Add(response);
                }
                else
                {
                    response.Value = responseViewModel.Value;
                }
            }

            ServiceResult result = await _userQuestionResponseRepository.SaveAsync(responses);

            return result.Success
                ? ServiceResult.Successful("Responses saved")
                : ServiceResult.Failure("Error saving responses");
        }
    }
}
