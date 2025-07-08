using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.VectorStorages.Hosts
{
    public class Host : ValueObject
    {
        public string Value { get; private set; } = "";

        public Host(string host)
        {
            CheckRule(new Rules.HostMustBeUrlOrLocal(host));

            Value = host;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}