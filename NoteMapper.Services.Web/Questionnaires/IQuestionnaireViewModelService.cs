using NoteMapper.Core;
using NoteMapper.Services.Web.ViewModels.Questionnaires;

namespace NoteMapper.Services.Web.Questionnaires
{
    public interface IQuestionnaireViewModelService
    {
        Task<QuestionnaireResponsesViewModel?> GetLatestQuestionnaireResponsesAsync(
            Guid userId);

        Task<ServiceResult> SaveResponsesAsync(Guid userId, 
            QuestionnaireResponsesViewModel responses);
    }
}
