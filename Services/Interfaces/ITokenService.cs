namespace BankProject;

public interface ITokenService
{
    public string GenerateToken(UserModel user);
}
