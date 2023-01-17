using Microsoft.AspNetCore.Components;
using NoteMapper.Core;
using NoteMapper.Data.Core.Users;
using NoteMapper.Services.Users;
using NoteMapper.Web.Blazor.Models;

namespace NoteMapper.Web.Blazor.Pages
{
    public abstract class NoteMapperComponentBase : ComponentBase
    {
        protected FeedbackViewModel? Feedback { get; set; }

        protected bool Loading { get; set; } = false;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Inject]
        protected IIdentityService IdentityService { get; set; }

        [Inject]
        protected IUserLocator UserLocator { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected Task<User?> GetCurrentUserAsync()
        {
            return UserLocator.GetCurrentUserAsync();
        }

        protected Task<Guid?> GetCurrentUserIdAsync()
        {
            return UserLocator.GetCurrentUserIdAsync();
        }

        protected void SetFeedback(ServiceResult result) 
        {
            SetData(FeedbackViewModel.FromServiceResult(result));
        }

        protected void SetError(string message)
        {
            SetData(new FeedbackViewModel
            {
                Message = message,
                Type = FeedbackType.Danger
            });
        }

        protected void SetLoading()
        {
            Loading = true;
            SetData(null);
        }

        private void SetData(FeedbackViewModel? feedback)
        {
            Feedback = feedback;
            if (feedback != null)
            {
                Loading = false;
            }
            InvokeAsync(() => StateHasChanged());
        }
    }
}
