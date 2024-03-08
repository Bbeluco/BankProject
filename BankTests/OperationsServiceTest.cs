using BankProject;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Moq;

namespace BankTests;

[TestClass]
public class OperationsServiceTest
{
    private Mock<IAssignorRepository> _stubRepoAssignor;
    private Mock<IPayableRepository> _stubRepoPayable;


    public OperationsServiceTest() {
        _stubRepoPayable = new Mock<IPayableRepository>();
        _stubRepoAssignor = new Mock<IAssignorRepository>();
    }

    [TestMethod]
    public void Payable_CorrectBodyDTO_ReturnsOkWithNewAssignorId()
    {
        PayableDTO dto = new PayableDTO() {
            Receivable = new Receivable() {
                Id = Guid.NewGuid().ToString(),
                Value = 10,
                Date = DateTime.UtcNow.AddDays(-3)
            },
            Assignor = new Assignor() {
                Document = "01124697012",
                Email = "a@a.com",
                Phone = "911111111",
                Name = "Fake"
            }
        };


        ReceivableModel mockReturnPayable = new ReceivableModel() {
            Id = Guid.NewGuid().ToString(),
            Value = 10,
            Date = new DateTime(2002, 02, 20),
            AssignorId = Guid.NewGuid().ToString()
        };

        AssignorModel mockReturnAssignor = new AssignorModel() {
            Document = dto.Assignor.Document,
            Email = dto.Assignor.Email,
            Phone = dto.Assignor.Phone,
            Name = dto.Assignor.Name
        };

        _stubRepoPayable.Setup(repo => repo.InsertReceivable(It.IsAny<ReceivableModel>()))
            .Returns(mockReturnPayable);

        _stubRepoAssignor.Setup(repo => repo.InsertAssignor(It.IsAny<AssignorModel>()))
            .Returns(mockReturnAssignor);

        var service = new OperationsService(_stubRepoAssignor.Object, _stubRepoPayable.Object);

        var result = service.Payable(dto);
        Assert.IsInstanceOfType(result.Result, typeof(Ok<ResponsePayableDTO>));

        ResponsePayableDTO response = ((Ok<ResponsePayableDTO>) result.Result).Value;
        Assert.IsNotNull(response);
        Assert.AreEqual(response.Assignor.Document, dto.Assignor.Document);
        Assert.AreEqual(response.Assignor.Phone, dto.Assignor.Phone);
        Assert.AreEqual(response.Assignor.Email, dto.Assignor.Email);
        Assert.AreEqual(response.Assignor.Name, dto.Assignor.Name);
        Assert.AreNotSame(response.Assignor.Id, "");
        Assert.AreEqual(response.Receivable, dto.Receivable);
    }

    [TestMethod]
    public void Payable_ReceivableIdNotGuid_ReturnsBadRequest() 
    {
        PayableDTO dto = new PayableDTO() {
            Receivable = new Receivable() {
                Id = "invalid",
                Value = 10,
                Date = DateTime.UtcNow.AddDays(-3)
            },
            Assignor = new Assignor() {
                Document = "01124697012",
                Email = "a@a.com",
                Phone = "911111111",
                Name = "Fake"
            }
        };

        OperationsService service = new OperationsService(_stubRepoAssignor.Object, 
            _stubRepoPayable.Object);
        
        var result = service.Payable(dto);
        
        Assert.IsInstanceOfType(result.Result, typeof(BadRequest<ErrorMessagesDTO>));
        ErrorMessagesDTO error = ((BadRequest<ErrorMessagesDTO>)result.Result).Value;
        Assert.AreEqual(error.ErrorMessage, "receivable.id is not an UUID");
    }

    [TestMethod]
    public void Payable_ReceivableIdDuplicated_ReturnsBadRequest() {
        PayableDTO dto = new PayableDTO() {
            Receivable = new Receivable() {
                Id = Guid.NewGuid().ToString(),
                Value = 10,
                Date = DateTime.UtcNow.AddDays(-3)
            },
            Assignor = new Assignor() {
                Document = "01124697012",
                Email = "a@a.com",
                Phone = "911111111",
                Name = "Fake"
            }
        };

        _stubRepoPayable.Setup(repo => repo.GetReceivableById(It.IsAny<string>()))
            .Returns(new ReceivableModel());

        OperationsService services = new OperationsService(_stubRepoAssignor.Object, _stubRepoPayable.Object);
        
        var result = services.Payable(dto);

        Assert.IsInstanceOfType(result.Result, typeof(Conflict<ErrorMessagesDTO>));

        ErrorMessagesDTO error = ((Conflict<ErrorMessagesDTO>) result.Result).Value;
        Assert.AreEqual(error.ErrorMessage, "receivable.id already exists");
    }

    [TestMethod]
    public void GetReceivableById_SearchInvalidId_ReturnsNotFound() {
        _stubRepoPayable.Setup(repo => repo.GetReceivableById(It.IsAny<string>()))
            .Returns(() => null);

        OperationsService service = new OperationsService(_stubRepoAssignor.Object, _stubRepoPayable.Object);
        var result = service.GetReceivableById("");

        Assert.IsInstanceOfType(result.Result, typeof(NotFound));
    }

    [TestMethod]
    public void GetReceivableById_SerchValidId_ReturnsOkAndReceivableModel() {
        _stubRepoPayable.Setup(repo => repo.GetReceivableById(It.IsAny<string>()))
            .Returns(new ReceivableModel());

        OperationsService service = new OperationsService(_stubRepoAssignor.Object, _stubRepoPayable.Object);
        var result = service.GetReceivableById(Guid.NewGuid().ToString());

        Assert.IsInstanceOfType(result.Result, typeof(Ok<ReceivableModel>));
    }

    [TestMethod]
    public void GetAssignorById_SearchInvalidId_ReturnsNotFound() {
        _stubRepoAssignor.Setup(repo => repo.GetAssignorById(It.IsAny<string>()))
            .Returns(() => null);

        OperationsService service = new OperationsService(_stubRepoAssignor.Object, _stubRepoPayable.Object);

        var result = service.GetAssignorById("");

        Assert.IsInstanceOfType(result.Result, typeof(NotFound));
    }

    [TestMethod]
    public void GetAssignorById_SearchValidId_ReturnsOkAndAssignorModel() {
        _stubRepoAssignor.Setup(repo => repo.GetAssignorById(It.IsAny<string>()))
            .Returns(new AssignorModel());

        OperationsService service = new OperationsService(_stubRepoAssignor.Object, _stubRepoPayable.Object);

        var result = service.GetAssignorById(Guid.NewGuid().ToString());

        Assert.IsInstanceOfType(result.Result, typeof(Ok<AssignorModel>));
    }

    [TestMethod]
    public void UpdateReceivable_TryUpdateInvalidId_ReturnsNotFound() {
        _stubRepoPayable.Setup(repo => repo.GetReceivableById(It.IsAny<string>()))
            .Returns(() => null);

        OperationsService service = new OperationsService(_stubRepoAssignor.Object, _stubRepoPayable.Object);

        var result = service.UpdateReceivable("", new ReceivableEditDTO());

        Assert.IsInstanceOfType(result.Result, typeof(NotFound));
    }

    [TestMethod]
    public void UpdateReceivable_UpdateValidId_ReturnsOkAndNewInfo() {
        ReceivableModel stubModel = new ReceivableModel() {
            Id = Guid.NewGuid().ToString(),
            AssignorId = Guid.NewGuid().ToString(),
            Date = DateTime.UtcNow,
            Value = 10
        };


        ReceivableEditDTO stubEditModel = new ReceivableEditDTO() {
            Date = DateTime.UtcNow.AddDays(-15),
            Value = 100
        };

        _stubRepoPayable.Setup(repo => repo.GetReceivableById(It.IsAny<string>()))
            .Returns(stubModel);

        _stubRepoPayable.Setup(repo => repo.UpdateReceivable(It.IsAny<ReceivableModel>(), It.IsAny<ReceivableEditDTO>()))
            .Returns(stubModel);

        OperationsService service = new OperationsService(_stubRepoAssignor.Object, _stubRepoPayable.Object);
        var result = service.UpdateReceivable(Guid.NewGuid().ToString(), stubEditModel);
        
        Assert.IsInstanceOfType(result.Result, typeof(Ok<ReceivableModel>));
    }

    [TestMethod]
    public void UpdateAssignor_TryUpdateInvalidId_ReturnsNotFound() {
        _stubRepoAssignor.Setup(repo => repo.GetAssignorById(It.IsAny<string>()))
            .Returns(() => null);

        OperationsService service = new OperationsService(_stubRepoAssignor.Object, _stubRepoPayable.Object);

        var result = service.UpdateAssignor("", new AssignorEditDTO());

        Assert.IsInstanceOfType(result.Result, typeof(NotFound));
    }

    [TestMethod]
    public void UpdateAssignor_UpdateValidId_ReturnsOkAndNewInfo() {
        AssignorModel stubModel = new AssignorModel();
        AssignorEditDTO stubEditModel = new AssignorEditDTO();

        _stubRepoAssignor.Setup(repo => repo.GetAssignorById(It.IsAny<string>()))
            .Returns(stubModel);

        _stubRepoAssignor.Setup(repo => repo.UpdateAssignor(It.IsAny<AssignorModel>(), It.IsAny<AssignorEditDTO>()))
            .Returns(stubModel);

        OperationsService service = new OperationsService(_stubRepoAssignor.Object, _stubRepoPayable.Object);

        var result = service.UpdateAssignor(Guid.NewGuid().ToString(), stubEditModel);

        Assert.IsInstanceOfType(result.Result, typeof(Ok<AssignorModel>));
    }

    [TestMethod]
    public void DeleteReceivable_DeleteId_ReturnsNoContent() {
        _stubRepoPayable.Setup(repo => repo.DeleteReceivable(It.IsAny<string>()))
            .Returns(() => null);

        OperationsService service = new OperationsService(_stubRepoAssignor.Object, _stubRepoPayable.Object);
        
        var result = service.DeleteAssignor(Guid.NewGuid().ToString());

        Assert.IsInstanceOfType(result.Result, typeof(NoContent));
    }

    [TestMethod]
    public void DeleteAssignor_DeleteId_ReturnsNoContent() {
        _stubRepoAssignor.Setup(repo => repo.DeleteAssignor(It.IsAny<string>()))
            .Returns(() => null);

        OperationsService service = new OperationsService(_stubRepoAssignor.Object, _stubRepoPayable.Object);
        
        var result = service.DeleteAssignor(Guid.NewGuid().ToString());

        Assert.IsInstanceOfType(result.Result, typeof(NoContent));
    }
}