namespace BankProject;

public interface IUserRepository
{
    public UserModel GetUser(LoginDTO dto);
    public UserModel RegisterUser(LoginDTO dto);
}
