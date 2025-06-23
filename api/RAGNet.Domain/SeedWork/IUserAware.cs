using RAGNET.Domain.Users;

namespace RAGNET.Domain.SeedWork
{
    public interface IUserAware
    {
        void InjectUser(User user);
    }

}