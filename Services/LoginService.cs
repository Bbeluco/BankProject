using Microsoft.AspNetCore.Http.HttpResults;

namespace BankProject;

public class LoginService : ILoginService
{
    public ITokenService _tokenService;
    public IUserRepository _userRepository;

    public LoginService(ITokenService tokenService, IUserRepository userRepository) {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public async Task<IResult> Login(LoginDTO dto)
    {
        UserModel user = _userRepository.GetUser(dto);
        if(user == null) {
            return Results.Unauthorized();
        }

        return TypedResults.Ok(new AuthenticationDTO() {
            User = dto.Login,
            Token = _tokenService.GenerateToken(user)
        });
    }

    public IResult Register(LoginDTO dto)
    {
        return TypedResults.Ok(_userRepository.RegisterUser(dto));
    }
}
