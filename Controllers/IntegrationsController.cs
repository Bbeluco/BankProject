using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankProject;

[ApiController]
public class IntegrationsController: ControllerBase
{

    public IOperationsService _operationsService;

    public IntegrationsController(IOperationsService payableService) {
        _operationsService = payableService;
    }

    [HttpPost("[controller]/payable")]
    public async Task<IResult> Payable([FromBody] PayableDTO dto) {
        return await _operationsService.Payable(dto);
    }

    [HttpGet("[controller]/payable/{id}")]
    public async Task<IResult> GetPayable(string id) {
        return await _operationsService.GetReceivableById(id);
    }

    [HttpGet("[controller]/assignor/{id}")]
    public Task<IResult> GetAssignor(string id) {
        return _operationsService.GetAssignorById(id);
    }

    [HttpPut("[controller]/assignor/{id}")]
    public Task<IResult> UpdateAssignor(string id, [FromBody] AssignorEditDTO dto) {
        return _operationsService.UpdateAssignor(id, dto);
    }

    [HttpPut("[controller]/payable/{id}")]
    public Task<IResult> UpdateReceivable(string id, [FromBody] ReceivableEditDTO dto) {
        return _operationsService.UpdateReceivable(id, dto);
    }

    [HttpDelete("[controller]/assignor/{id}")]
    public Task<IResult> DeleteAssignor(string id) {
        return _operationsService.DeleteAssignor(id);
    }

    [HttpDelete("[controller]/payable/{id}")]
    public Task<IResult> DeleteReceivable(string id) {
        return _operationsService.DeleteReceivable(id);
    }
}
