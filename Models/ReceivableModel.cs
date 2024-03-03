namespace BankProject;

public class ReceivableModel
{
    public string Id { get; set; }
    public double Value { get; set; }
    public DateTime Date { get; set; }

    //Foreign key reference
    public string AssignorId { get; set; }
    public AssignorModel Assignor { get; set; }
}
