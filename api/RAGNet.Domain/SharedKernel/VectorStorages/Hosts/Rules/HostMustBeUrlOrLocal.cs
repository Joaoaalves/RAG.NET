using System.Text.RegularExpressions;
using RAGNET.Domain.SeedWork;

namespace RAGNET.Domain.SharedKernel.VectorStorages.Hosts.Rules
{
    public class HostMustBeUrlOrLocal(string host) : IBusinessRule
    {
        private readonly string _host = host;

        public string Message => "The host must be an valid URL or must be local";
        public static string Pattern => @"(?!(ftp|http|https):\/\/)(?!www)[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,}";
        public bool IsBroken() => !Regex.IsMatch(_host, Pattern);
    }
}