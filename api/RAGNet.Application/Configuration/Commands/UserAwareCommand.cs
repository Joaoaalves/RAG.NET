using RAGNET.Domain.SeedWork;
using RAGNET.Domain.Users;

namespace RAGNET.Application.Configuration.Commands
{
    public abstract class UserAwareCommand<TResult> : IUserAware, ICommand<TResult>
    {
        public Guid Id => Guid.NewGuid();
        public User User { get; private set; } = default!;

        public void InjectUser(User user)
        {
            User = user;
        }
    }

}