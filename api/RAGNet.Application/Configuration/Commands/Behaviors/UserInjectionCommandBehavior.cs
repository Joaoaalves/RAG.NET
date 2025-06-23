using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Users;

namespace RAGNET.Application.Configuration.Commands.Behaviors
{
    public class UserInjectionCommandBehavior<TCommand, TResult>(
        IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager)
        : ICommandPipelineBehavior<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<TResult> Handle(
            TCommand command,
            Func<TCommand, Task<TResult>> next,
            CancellationToken cancellationToken)
        {
            if (command is IUserAware userAware)
            {
                var claimsPrincipal = _httpContextAccessor.HttpContext?.User
                    ?? throw new Exception("User not found in context.");

                var user = await _userManager.GetUserAsync(claimsPrincipal)
                    ?? throw new Exception("Failed to resolve authenticated user.");

                userAware.InjectUser(user);
            }

            return await next(command);
        }
    }
}
