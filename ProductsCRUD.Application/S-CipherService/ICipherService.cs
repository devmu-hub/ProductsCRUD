namespace ProductsCRUD.Application.S_CipherService
{
    public interface ICipherService
    {
        string Encrypt(string input);
        string Decrypt(string cipherText);
    }
}
