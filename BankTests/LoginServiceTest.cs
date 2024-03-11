using BankProject;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BankTests;

[TestClass]
public class LoginServiceTest
{
    private Mock<ITokenService> _tokenServiceStub;
    private Mock<IUserRepository> _userRepositoryStub;

    public LoginServiceTest() {
        _tokenServiceStub = new Mock<ITokenService>();
        _userRepositoryStub = new Mock<IUserRepository>();
    }

    [TestMethod]
    public void Login_PassValidUser_ReturnsToken() {
        LoginDTO dto = new LoginDTO() {
            Login = "unit",
            Password = "test"
        };
        _tokenServiceStub.Setup(token => token.GenerateToken(It.IsAny<UserModel>()))
            .Returns("token");

        _userRepositoryStub.Setup(repo => repo.GetUser(It.IsAny<LoginDTO>()))
            .Returns(new UserModel() { 
                Id = 1,
                User = "unit test", 
                Password = "unit", 
                Role = "admin"
            });

        LoginService service = new LoginService(_tokenServiceStub.Object, _userRepositoryStub.Object);
        var result = service.Login(dto);

        Assert.IsInstanceOfType(result.Result, typeof(Ok<AuthenticationDTO>));

        AuthenticationDTO response = ((Ok<AuthenticationDTO>) result.Result).Value;
        Assert.IsNotNull(response.User);
        Assert.IsNotNull(response.Token);
    }

    [TestMethod]
    public void Login_PassInvalidUser_ReturnsUnauthorized() {
        _userRepositoryStub.Setup(repo => repo.GetUser(It.IsAny<LoginDTO>()))
            .Returns(() => null);

        LoginService service = new LoginService(_tokenServiceStub.Object, _userRepositoryStub.Object);

        var result = service.Login(new LoginDTO());

        Assert.IsInstanceOfType(result.Result, typeof(UnauthorizedHttpResult));
    }

    [TestMethod]
    public void Register_returnsStatusOk() {
        _userRepositoryStub.Setup(repo => repo.RegisterUser(It.IsAny<LoginDTO>()))
            .Returns(new UserModel() { 
                Id = 1,
                User = "unit test", 
                Password = "unit", 
                Role = "admin"
            });

        LoginService service = new LoginService(_tokenServiceStub.Object, _userRepositoryStub.Object);

        var result = service.Register(new LoginDTO());

        Assert.IsInstanceOfType(result, typeof(Ok<UserModel>));
    }
}
