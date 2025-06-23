using RAGNET.Application.Configuration.ExecutionContext;
using RAGNET.Domain.Users;

namespace RAGNET.Application.Configuration.Queries
{
    public class UserAwareQuery<TResult> : IUserAware, IQuery<TResult>
    {
        public User User { get; private set; } = default!;

        public void InjectUser(User user)
        {
            User = user;
        }
    }
}