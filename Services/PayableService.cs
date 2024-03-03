
using System.Reflection;
using System.Xml.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.ObjectPool;
using Microsoft.VisualBasic;

namespace BankProject;


public class PayableService : IPayableService
{
    private IAssignorRepository _assignorRepository { get; set; }
    private IPayableRepository _payableRepository { get; set; }

    public PayableService(IAssignorRepository assignorRepository, IPayableRepository payableRepository) {
        _assignorRepository = assignorRepository;
        _payableRepository = payableRepository;
    }

    public async Task<IResult> Payable(PayableDTO dto)
    {
        Guid guidOutput = new Guid();
        bool isValid = Guid.TryParse(dto.Receivable.Id, out guidOutput);
        if(!isValid) {
            return Results.BadRequest(new ErrorMessagesDTO { ErrorMessage = "receivable.id is not an UUID" });
        }

        AssignorModel assignor = new AssignorModel() {
            Document = dto.Assignor.Document,
            Email = dto.Assignor.Email,
            Phone = dto.Assignor.Phone,
            Name = dto.Assignor.Name
        };

        var newAssignor = _assignorRepository.InsertAssignor(assignor);

        ReceivableModel receivable = new ReceivableModel() {
            Id = dto.Receivable.Id,
            Value = dto.Receivable.Value,
            Date = dto.Receivable.Date,
            AssignorId = newAssignor.AssignorId
        };

        _payableRepository.InsertReceivable(receivable);

        ResponsePayableDTO resp = new ResponsePayableDTO(dto, newAssignor.AssignorId);

        return Results.Ok(resp);
    }
}
