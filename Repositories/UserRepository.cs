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
            .First();

        return userDb;
    }
}
