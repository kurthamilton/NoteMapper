using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using NoteMapper.Data.Core.Questionnaires;
using NoteMapper.Services.Web.ViewModels.DataAnnotations;

namespace NoteMapper.Services.Web.ViewModels.Questionnaires
{
    public class QuestionnaireResponseViewModel
    {
        private static Regex NumberRangeRegex = new Regex(@"^(?<min>-?\d+)-(?<max>-?\d+)$", RegexOptions.Compiled);

        public QuestionnaireResponseViewModel(QuestionnaireQuestion question, 
            UserQuestionResponse? response)
        {
            QuestionId = question.QuestionId;
            QuestionText = question.QuestionText;
            QuestionType = question.Type;            
            Required = question.Required;
            Value = response?.Value ?? "";

            if (QuestionType == QuestionType.Number && !string.IsNullOrEmpty(question.Range))
            {
                Match match = NumberRangeRegex.Match(question.Range);
                if (match.Success)
                {
                    MinValue = int.Parse(match.Groups["min"].Value);
                    MaxValue = int.Parse(match.Groups["max"].Value);
                }
            }
        }

        public int? MaxValue { get; }

        public int? MinValue { get; }

        public Guid QuestionId { get; }

        public string QuestionText { get; }

        public QuestionType QuestionType { get; }

        public bool Required { get; }

        [RequiredIf(nameof(Required))]
        public string Value { get; set; }

        public int ValueNumber
        {
            get
            {
                int.TryParse(Value, out int intValue);
                return intValue;
            }
            set => Value = value.ToString();
        }
    }
}
