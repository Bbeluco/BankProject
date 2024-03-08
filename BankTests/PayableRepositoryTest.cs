using System.Diagnostics.Contracts;
using BankProject;
using Microsoft.EntityFrameworkCore;

namespace BankTests;

[TestClass]
public class PayableRepositoryTest
{
    private string _searchableId = Guid.NewGuid().ToString();
    private DbContextOptions<BankAppContext> _options;

    public PayableRepositoryTest() {
        _options = new DbContextOptionsBuilder<BankAppContext>()
        .UseInMemoryDatabase(databaseName: "BankApp").Options;

        using (var context = new BankAppContext(_options)) {
            context.Receivable.Add(new ReceivableModel { Id = _searchableId, Date = DateTime.UtcNow, Value = 1, AssignorId = Guid.NewGuid().ToString() });
            context.Receivable.Add(new ReceivableModel { Id = Guid.NewGuid().ToString(), Date = DateTime.UtcNow, Value = 2, AssignorId = Guid.NewGuid().ToString() });
            context.Receivable.Add(new ReceivableModel { Id = Guid.NewGuid().ToString(), Date = DateTime.UtcNow, Value = 3, AssignorId = Guid.NewGuid().ToString() });
            context.SaveChanges();
        }
    }

    [TestMethod]
    public void GetReceivableById_SearchInvalidId_ReturnsNull() {
        using(var context = new BankAppContext(_options)) {
            PayableRepository repository = new PayableRepository(context);
            var result = repository.GetReceivableById("invalid");

            Assert.IsNull(result);
        }
    }

    [TestMethod]
    public void GetReceivableById_SearchValidId_ReturnsNotNullModel() {
        using(var context = new BankAppContext(_options)) {
            PayableRepository repository = new PayableRepository(context);

            var result = repository.GetReceivableById(_searchableId);
            Assert.IsNotNull(result);
        }
    }

    [TestMethod]
    public void InsertReceivable_ShouldReturnModelCreated() {
        using(var context = new BankAppContext(_options)) {
            ReceivableModel model = context.Receivable.Find(_searchableId);
            string newGuid = Guid.NewGuid().ToString();
            model.Id = newGuid;

            PayableRepository repository = new PayableRepository(context);
            var result = repository.InsertReceivable(model);

            Assert.IsNotNull(result);
        }
    }

    [TestMethod]
    public void InsertReceivable_ShouldSaveModelInDb() {
        using(var context = new BankAppContext(_options)) {
            ReceivableModel model = context.Receivable.Find(_searchableId);
            string newGuid = Guid.NewGuid().ToString();
            model.Id = newGuid;

            PayableRepository repository = new PayableRepository(context);
            repository.InsertReceivable(model);

            ReceivableModel insertedModel = context.Receivable.Find(newGuid);
            Assert.IsNotNull(insertedModel);
        }
    }

    [TestMethod]
    public void UpdateReceivable_PassAllOptionalValues_ReturnsModelWithNewValues() {
        ReceivableEditDTO editDTO = new ReceivableEditDTO() {
            Date = DateTime.UtcNow.AddDays(-3),
            Value = 100
        };

        using (var context = new BankAppContext(_options)) {
            PayableRepository repository = new PayableRepository(context);

            ReceivableModel model = context.Receivable.Find(_searchableId);

            var result = repository.UpdateReceivable(model, editDTO);
            Assert.IsTrue(result.Id == model.Id);
            Assert.IsTrue(result.Value == editDTO.Value);
            Assert.IsTrue(result.Date == editDTO.Date);
        }
    }

    [TestMethod]
    public void UpdateReceivable_NotPassAnyChangeInEditDTO_ReturnsSameInput() {
        ReceivableEditDTO editDTO = new ReceivableEditDTO();

        using(var context = new BankAppContext(_options)) {
            ReceivableModel model = context.Receivable.Find(_searchableId);

            PayableRepository repository = new PayableRepository(context);
            var result = repository.UpdateReceivable(model, editDTO);

            Assert.AreEqual(model, result);
        }
    }

    [TestMethod]
    public void UpdateReceivable_UpdateModelValues_ShouldSaveInfoInDB() {
        ReceivableEditDTO editDTO = new ReceivableEditDTO() {
            Date = DateTime.UtcNow.AddDays(-3),
            Value = 100
        };

        using(var context = new BankAppContext(_options)) {
            PayableRepository repository = new PayableRepository(context);
            ReceivableModel model = context.Receivable.Find(_searchableId);

            repository.UpdateReceivable(model, editDTO);
            ReceivableModel updatedModel = context.Receivable.Find(_searchableId);

            Assert.IsTrue(updatedModel.Value == editDTO.Value);
            Assert.IsTrue(updatedModel.Date == editDTO.Date);
        }

    }

    [TestMethod]
    public void DeleteReceivable_ReturnModel() {
        using(var context = new BankAppContext(_options)) {
            PayableRepository repository = new PayableRepository(context);
            ReceivableModel model = context.Receivable.Find(_searchableId);
            var result = repository.DeleteReceivable(_searchableId);

            Assert.AreEqual(model, result);
        }
    }

    [TestMethod]
    public void DeleteReceivable_ReturnsNullForSearchOldRow() {
        using(var context = new BankAppContext(_options)) {
            PayableRepository repository = new PayableRepository(context);
            var result = repository.DeleteReceivable(_searchableId);
            ReceivableModel model = context.Receivable.Find(_searchableId);

            Assert.IsNull(model);
        }
    }
}
