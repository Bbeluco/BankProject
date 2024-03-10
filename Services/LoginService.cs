namespace BankProject;

public class LoginService : ILoginService
{
    public ITokenService _tokenService;
    public IUserRepository _userRepository;

    public LoginService(ITokenService tokenService, IUserRepository userRepository) {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public IResult Login(LoginDTO dto)
    {
        UserModel user = _userRepository.GetUser(dto);
        if(user == null) {
            return Results.Unauthorized();
        }

        return Results.Ok(new AuthenticationDTO() {
            User = dto.Login,
            Token = _tokenService.GenerateToken(user)
        });
    }
}
