namespace BankProject;

public class AssignorRepository : IAssignorRepository
{
    private BankAppContext _bankAppContext;

    public AssignorRepository(BankAppContext bankAppContext) {
        _bankAppContext = bankAppContext;
    }

    public AssignorModel DeleteAssignor(string id)
    {
        AssignorModel assignor = GetAssignorById(id);
        _bankAppContext.Remove(assignor);
        _bankAppContext.SaveChanges();
        return assignor;
    }

    public AssignorModel GetAssignorById(string id)
    {
        return _bankAppContext.Assignor.Find(id);
    }

    public AssignorModel InsertAssignor(AssignorModel assignor)
    {
        assignor.AssignorId = Guid.NewGuid().ToString();
        _bankAppContext.Assignor.Add(assignor);
        _bankAppContext.SaveChanges();

        return assignor;
    }

    public AssignorModel UpdateAssignor(AssignorModel assignor, AssignorEditDTO dto)
    {
        assignor.Document = dto.Document ?? assignor.Document;
        assignor.Email = dto.Email ?? assignor.Email;
        assignor.Phone = dto.Phone ?? assignor.Phone;
        assignor.Name = dto.Name ?? assignor.Phone;

        _bankAppContext.Update(assignor);
        _bankAppContext.SaveChanges();

        return assignor;       
    }
}
