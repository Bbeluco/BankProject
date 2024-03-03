
namespace BankProject;

public class PayableRepository : IPayableRepository
{
    private BankAppContext _bankAppContext;
    public PayableRepository(BankAppContext bankAppContext) {
        _bankAppContext = bankAppContext;
    }

    public ReceivableModel InsertReceivable(ReceivableModel receivable)
    {
        _bankAppContext.Receivable.Add(receivable);
        _bankAppContext.SaveChanges();

        return receivable;
    }
}
