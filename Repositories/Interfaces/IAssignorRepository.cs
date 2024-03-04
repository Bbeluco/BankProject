namespace BankProject;

public interface IAssignorRepository
{
    public AssignorModel InsertAssignor(AssignorModel assignor);
    public AssignorModel GetAssignorById(string id);
    public AssignorModel UpdateAssignor(AssignorModel assignor, AssignorEditDTO dto);
    public AssignorModel DeleteAssignor(string id);
}
