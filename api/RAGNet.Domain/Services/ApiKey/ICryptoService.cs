namespace RAGNET.Domain.Services.ApiKey
{
    public interface ICryptoService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}