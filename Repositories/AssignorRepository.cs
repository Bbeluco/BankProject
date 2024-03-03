namespace BankProject;

public class AssignorRepository : IAssignorRepository
{
    private BankAppContext _bankAppContext;

    public AssignorRepository(BankAppContext bankAppContext) {
        _bankAppContext = bankAppContext;
    }

    public AssignorModel InsertAssignor(AssignorModel assignor)
    {
        assignor.AssignorId = Guid.NewGuid().ToString();
        _bankAppContext.Assignor.Add(assignor);
        _bankAppContext.SaveChanges();

        return assignor;
    }
}
