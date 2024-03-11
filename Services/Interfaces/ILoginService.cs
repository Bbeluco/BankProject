namespace BankProject;

public interface ILoginService
{
    public IResult Login(LoginDTO dto);
    public IResult Register(LoginDTO dto);
}
