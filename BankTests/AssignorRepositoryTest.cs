using BankProject;
using Microsoft.EntityFrameworkCore;

namespace BankTests;

[TestClass]
public class AssignorRepositoryTest
{
    private string _searchableId = Guid.NewGuid().ToString();
    private DbContextOptions<BankAppContext> _options;

    public AssignorRepositoryTest() {
        _options = new DbContextOptionsBuilder<BankAppContext>()
            .UseInMemoryDatabase(databaseName: "BankDb").Options;

        _searchableId = Guid.NewGuid().ToString();

        using(var context = new BankAppContext(_options)) {
            context.Assignor.Add(new AssignorModel { AssignorId = _searchableId, Name = "Fake", Document = "Fake", Email = "Fake", Phone = "Fake" });
            context.Assignor.Add(new AssignorModel { AssignorId = Guid.NewGuid().ToString(), Name = "Fake", Document = "Fake", Email = "Fake", Phone = "Fake" });
            context.Assignor.Add(new AssignorModel { AssignorId = Guid.NewGuid().ToString(), Name = "Fake", Document = "Fake", Email = "Fake", Phone = "Fake" });
            context.SaveChanges();
        }
    }

    [TestMethod]
    public void GetAssignorById_SearchInvalidId_ReturnsNull() {
        using(var context = new BankAppContext(_options)) {
            AssignorRepository repository = new AssignorRepository(context);
            var result = repository.GetAssignorById("invalid");

            Assert.IsNull(result);
        }
    }
    
    [TestMethod]
    public void GetAssignorById_SearchValidId_ReturnsNotNullModel() {
        using(var context = new BankAppContext(_options)) {
            AssignorRepository repository = new AssignorRepository(context);
            var result = repository.GetAssignorById(_searchableId);

            Assert.IsNotNull(result);
        }
    }

    [TestMethod]
    public void InsertAssignor_ShouldReturnModelCreated() {
        AssignorModel model = new AssignorModel {
            Name = "insert test", 
            Document = "33120975095", 
            Email = "unit.test@email.com", 
            Phone = "6820282693" 
        };

        using(var context = new BankAppContext(_options)) {
            AssignorRepository repository = new AssignorRepository(context);
            var result = repository.InsertAssignor(model);
            
            Assert.IsTrue(result.Name == model.Name);
            Assert.IsTrue(result.Document == model.Document);
            Assert.IsTrue(result.Email == model.Email);
            Assert.IsTrue(result.Phone == model.Phone);
        }
    }

    [TestMethod]
    public void InsertAssignor_ShouldSaveModelInDb() {
        AssignorModel model = new AssignorModel {
            Name = "Fake", 
            Document = "Fake", 
            Email = "Fake", 
            Phone = "Fake" 
        };

        using(var context = new BankAppContext(_options)) {
            AssignorRepository repository = new AssignorRepository(context);
            var result = repository.InsertAssignor(model);

            AssignorModel insertedModel = context.Assignor.Find(result.AssignorId);
            Assert.IsNotNull(insertedModel);
        }
    }

    [TestMethod]
    public void UpdateAssignor_PassAllOptionalValues_ReturnsModelWithNewValues() {
        AssignorEditDTO editDTO = new AssignorEditDTO() {
            Phone = "7932503128",
            Document = "04688420048",
            Email = "test@test.com",
            Name = "unit test"
        };

        using(var context = new BankAppContext(_options)) {
            AssignorRepository repository = new AssignorRepository(context);
            AssignorModel model = context.Assignor.Find(_searchableId);

            var result = repository.UpdateAssignor(model, editDTO);

            Assert.IsTrue(result.Name == editDTO.Name);
            Assert.IsTrue(result.Document == editDTO.Document);
            Assert.IsTrue(result.Phone == editDTO.Phone);
            Assert.IsTrue(result.Email == editDTO.Email);
        }
    }

    [TestMethod]
    public void UpdateAssignor_NotPassAnyChangeInEditDTO_ReturnsSameInput() {
        AssignorEditDTO editDTO = new AssignorEditDTO();

        using(var context = new BankAppContext(_options)) {
            AssignorRepository repository = new AssignorRepository(context);
            AssignorModel model = context.Assignor.Find(_searchableId);

            var result = repository.UpdateAssignor(model, editDTO);

            Assert.AreEqual(model, result);
        }
    }

    [TestMethod]
    public void UpdateAssignor_UpdateModelValues_ShouldSaveInfoInDB() {
        AssignorEditDTO editDTO = new AssignorEditDTO() {
            Phone = "7932503128",
            Document = "04688420048",
            Email = "test@test.com",
            Name = "unit test"
        };

        using(var context = new BankAppContext(_options)) {
            AssignorRepository repository = new AssignorRepository(context);
            AssignorModel model = context.Assignor.Find(_searchableId);

            repository.UpdateAssignor(model, editDTO);
            AssignorModel updatedAssignorModel = context.Assignor.Find(_searchableId);
            Assert.IsTrue(updatedAssignorModel.Name == editDTO.Name);
            Assert.IsTrue(updatedAssignorModel.Phone == editDTO.Phone);
            Assert.IsTrue(updatedAssignorModel.Document == editDTO.Document);
            Assert.IsTrue(updatedAssignorModel.Email == editDTO.Email);
        }
    }

    [TestMethod]
    public void DeleteAssignor_ReturnModel() {
        using(var context = new BankAppContext(_options)) {
            AssignorRepository repository = new AssignorRepository(context);
            AssignorModel model = context.Assignor.Find(_searchableId);
            var result = repository.DeleteAssignor(_searchableId);

            Assert.AreEqual(model, result);
        }
    }

    [TestMethod]
    public void DeleteAssignor_ReturnsNullForSearchOldRow() {
        using(var context = new BankAppContext(_options)) {
            AssignorRepository repository = new AssignorRepository(context);
            var result = repository.DeleteAssignor(_searchableId);
            AssignorModel model = context.Assignor.Find(_searchableId);

            Assert.IsNull(model);
        }
    }
}
