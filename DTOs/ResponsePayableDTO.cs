namespace BankProject;

public class ResponsePayableDTO
{
    public Receivable Receivable { get; set; }
    public AssignorResponse Assignor { get; set; } = new AssignorResponse();

    public ResponsePayableDTO(PayableDTO dto, string assignorId) {
        Receivable = dto.Receivable;
        
        Assignor.Document = dto.Assignor.Document;
        Assignor.Email = dto.Assignor.Email;
        Assignor.Phone = dto.Assignor.Phone;
        Assignor.Name = dto.Assignor.Name;
        Assignor.Id = assignorId;
    }
}

public class AssignorResponse : Assignor {
    public string Id { get; set; }
}