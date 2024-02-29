using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankProject;

[ApiController]
[Route("integrations")]
public class IntegrationsController: ControllerBase
{

    public IPayableService _payableService;

    public IntegrationsController(IPayableService payableService) {
        _payableService = payableService;
    }

    [HttpPost("/payable")]
    public async Task<IResult> Payable([FromBody] PayableDTO dto) {
        return await _payableService.Payable(dto);
    }
}
