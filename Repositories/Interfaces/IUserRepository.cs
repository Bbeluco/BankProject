namespace BankProject;

public interface IUserRepository
{
    public UserModel GetUser(LoginDTO dto);
}
