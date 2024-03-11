namespace BankProject;

public interface ILoginService
{
    public Task<IResult> Login(LoginDTO dto);
    public IResult Register(LoginDTO dto);
}
