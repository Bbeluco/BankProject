using BankProject;
using Microsoft.EntityFrameworkCore;

namespace BankTests;

[TestClass]
public class UserRepositoryTest
{
    private DbContextOptions<BankAppContext> _options;

    public UserRepositoryTest() {
        _options = new DbContextOptionsBuilder<BankAppContext>()
            .UseInMemoryDatabase(databaseName: "BankApp").Options;

        using(var context = new BankAppContext(_options)) {
            context.User.Add(new UserModel() {
                User = "admin",
                Password = "password",
                Role = "admin"
            });

            context.SaveChanges();
        }
    }

    [TestMethod]
    public void GetUser_SearchForValidUserAndPass_ReturnsUserModelNotNull() {
        using(var context = new BankAppContext(_options)) {
            LoginDTO dto = new LoginDTO() {
                Login = "admin",
                Password = "password"
            };

            UserRepository repository = new UserRepository(context);

            var result = repository.GetUser(dto);
            Assert.IsNotNull(result); 
        }
    }

    [TestMethod]
    public void GetUser_SearchForInvalidUser_ReturnsNull() {
        using(var context = new BankAppContext(_options)) {
            LoginDTO dto = new LoginDTO() {
                Login = "not exists",
                Password = "password"
            };

            UserRepository repository = new UserRepository(context);
            var result = repository.GetUser(dto);

            Assert.IsNull(result);
        }
    }

    [TestMethod]
    public void RegisterUser_InputInfo_CheckIfSavesInDb() {
        using(var context = new BankAppContext(_options)) {
            LoginDTO dto = new LoginDTO() {
                Login = "unit",
                Password = "test"
            };

            UserRepository repository = new UserRepository(context);
            repository.RegisterUser(dto);

            var result = repository.GetUser(dto);
            Assert.IsNotNull(result);
        }
    }

    [TestMethod]
    public void RegisterUser_InputInfo_ReturnsUserModel() {
        using(var context = new BankAppContext(_options)) {
            LoginDTO dto = new LoginDTO() {
                Login = "unit",
                Password = "test"
            };

            UserRepository repository = new UserRepository(context);
            var result = repository.RegisterUser(dto);


            Assert.AreEqual(result.User, dto.Login);
            Assert.AreEqual(result.Password, dto.Password);
            Assert.AreEqual(result.Role, "admin");
        }
    }
}
