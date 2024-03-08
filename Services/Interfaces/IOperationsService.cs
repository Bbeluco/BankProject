using Microsoft.AspNetCore.Mvc;

namespace BankProject;

public interface IOperationsService
{
    public Task<IResult> Payable(PayableDTO dto);
    public Task<IResult> GetReceivableById(string id);
    public Task<IResult> GetAssignorById(string id);
    public Task<IResult> UpdateAssignor(string id, AssignorEditDTO dto);
    public Task<IResult> UpdateReceivable(string id, ReceivableEditDTO dto);
    public Task<IResult> DeleteAssignor(string id);
    public Task<IResult> DeleteReceivable(string id);
}
