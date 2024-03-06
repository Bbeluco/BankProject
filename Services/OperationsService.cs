
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.ObjectPool;
using Microsoft.VisualBasic;

namespace BankProject;


public class OperationsService : IOperationsService
{
    private IAssignorRepository _assignorRepository { get; set; }
    private IPayableRepository _payableRepository { get; set; }

    public OperationsService(IAssignorRepository assignorRepository, IPayableRepository payableRepository) {
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

        if(_payableRepository.GetReceivableById(dto.Receivable.Id) != null) {
            return Results.Conflict(new ErrorMessagesDTO { ErrorMessage = "receivable.id already exists" });
        }

        AssignorModel assignor = new AssignorModel() {
            Document = dto.Assignor.Document,
            Email = dto.Assignor.Email,
            Phone = dto.Assignor.Phone,
            Name = dto.Assignor.Name
        };

        var newAssignor = _assignorRepository.InsertAssignor(assignor);

        System.Console.WriteLine(newAssignor == null);
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

    public async Task<IResult> GetReceivableById(string id)
    {
        ReceivableModel t = _payableRepository.GetReceivableById(id);
        if(t == null) {
            return Results.NotFound();
        }

        return Results.Ok(t);
    }

    public async Task<IResult> GetAssignorById(string id)
    {
        AssignorModel a = _assignorRepository.GetAssignorById(id);
        if(a == null) {
            return Results.NotFound();
        }

        return Results.Ok(a);
    }

    public async Task<IResult> UpdateAssignor(string id, AssignorEditDTO dto)
    {
        AssignorModel assignor = _assignorRepository.GetAssignorById(id);
        if(assignor == null) {
            return Results.NotFound();
        }

        return Results.Ok(_assignorRepository.UpdateAssignor(assignor, dto));
    }

    public async Task<IResult> UpdateReceivable(string id, ReceivableEditDTO dto)
    {
        ReceivableModel receivable = _payableRepository.GetReceivableById(id);
        if(receivable == null) {
            return Results.NotFound();
        }

        return Results.Ok(_payableRepository.UpdateReceivable(receivable, dto));
    }

    public async Task<IResult> DeleteAssignor(string id)
    {
        return Results.Ok(_assignorRepository.DeleteAssignor(id));
    }

    public async Task<IResult> DeleteReceivable(string id)
    {
        _payableRepository.DeleteReceivable(id);
        return Results.NoContent();
    }
}
