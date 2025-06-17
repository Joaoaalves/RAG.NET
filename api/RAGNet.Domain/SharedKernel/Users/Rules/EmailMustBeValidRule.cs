using System.Net.Mail;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.Users.Rules
{
    public class EmailMustBeValidRule(string email) : IBusinessRule
    {
        private readonly string _email = email;

        public string Message => "The email must be a valid email address.";

        public bool IsBroken()
        {
            if (string.IsNullOrWhiteSpace(_email))
                return true;

            try
            {
                var addr = new MailAddress(_email);
                return addr.Address != _email;
            }
            catch
            {
                return true;
            }
        }
    }
}