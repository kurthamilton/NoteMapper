using System.ComponentModel.DataAnnotations;

namespace NoteMapper.Web.Blazor.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
