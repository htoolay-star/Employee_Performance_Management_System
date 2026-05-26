namespace EPMS.Domain.Interface.IService.Auth
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string passwordHash);
    }
}
