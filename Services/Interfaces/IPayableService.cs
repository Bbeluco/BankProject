namespace BankProject;

public interface IPayableService
{
    public Task<IResult> Payable(PayableDTO dto);
}
