using System.ComponentModel.DataAnnotations;

namespace NoteMapper.Services.Web.ViewModels.Questionnaires
{
    public class QuestionnaireResponsesViewModel
    {
        public QuestionnaireResponsesViewModel(Guid questionnaireId,
            IReadOnlyCollection<QuestionnaireResponseViewModel> responses)
        {
            QuestionnaireId = questionnaireId;
            Responses = responses.ToArray();
        }

        public Guid QuestionnaireId { get; }

        [ValidateComplexType]
        public QuestionnaireResponseViewModel[] Responses { get; }
    }
}
