using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BankProject;

public class UserRepository : IUserRepository
{
    private BankAppContext _bankAppContext;

    public UserRepository(BankAppContext bankAppContext) {
        _bankAppContext = bankAppContext;
    }

    public UserModel GetUser(LoginDTO dto)
    {
        UserModel userDb = _bankAppContext.User
            .Where(u => u.User == dto.Login && u.Password == dto.Password)
            .FirstOrDefault();

        return userDb;
    }

    public UserModel RegisterUser(LoginDTO dto)
    {
        UserModel user = new UserModel() {
            User = dto.Login,
            Password = dto.Password,
            Role = "admin"
        };

        _bankAppContext.User.Add(user);
        _bankAppContext.SaveChanges();

        return user;
    }
}
