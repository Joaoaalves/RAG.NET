using RAGNET.Domain.Users;

namespace RAGNET.Application.Configuration.ExecutionContext
{
    public interface IUserAware
    {
        void InjectUser(User user);
    }

}