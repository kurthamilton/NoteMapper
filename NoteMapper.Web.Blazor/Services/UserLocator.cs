using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using NoteMapper.Data.Core.Users;
using NoteMapper.Services.Users;

namespace NoteMapper.Web.Blazor.Services
{
    public class UserLocator : IUserLocator
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IUserRepository _userRepository;

        public UserLocator(IUserRepository userRepository, 
            AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _userRepository = userRepository;
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            AuthenticationState state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            ClaimsPrincipal principal = state.User;
            Claim? userIdClaim = principal.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim?.Value, out Guid userId))
            {
                return null;
            }

            User? user = await _userRepository.FindAsync(userId);
            return user;
        }
    }
}
