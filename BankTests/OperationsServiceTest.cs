using BankProject;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Moq;

namespace BankTests;

[TestClass]
public class OperationsServiceTest
{
    [TestMethod]
    public void TestPayableReturn()
    {
        PayableDTO dto = new PayableDTO() {
            Receivable = new Receivable() {
                Id = "0f91f7fa-ae35-4644-bc63-a89a0176dab6",
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
            Id = "0f91f7fa-ae35-4644-bc63-a89a0176dab6",
            Value = 10,
            Date = new DateTime(2002, 02, 20),
            AssignorId = "2"
        };

        AssignorModel mockReturnAssignor = new AssignorModel() {
            AssignorId = "2",
            Document = "12312312312",
            Email = "test@test.com",
            Phone = "911111111",
            Name = "mock test"
        };
        Mock<IPayableRepository> mockRepoPayable = new Mock<IPayableRepository>();
        Mock<IAssignorRepository> mockRepoAssignor = new Mock<IAssignorRepository>();

        mockRepoPayable.Setup(repo => repo.InsertReceivable(mockReturnPayable))
            .Returns(mockReturnPayable);

        mockRepoAssignor.Setup(repo => repo.InsertAssignor(mockReturnAssignor))
            .Returns(mockReturnAssignor);
        var service = new OperationsService(mockRepoAssignor.Object, mockRepoPayable.Object);

        var result = service.Payable(dto);

        Assert.Equals(result.Result, "123");
    }
}