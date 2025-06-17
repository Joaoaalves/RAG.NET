namespace RAGNET.Application.ApiKeys
{
    public interface ICryptoService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}