
namespace BankProject;

public class PayableRepository : IPayableRepository
{
    private BankAppContext _bankAppContext;
    public PayableRepository(BankAppContext bankAppContext) {
        _bankAppContext = bankAppContext;
    }

    public ReceivableModel DeleteReceivable(string id)
    {
        ReceivableModel receivable = GetReceivableById(id);
        _bankAppContext.Remove(receivable);
        _bankAppContext.SaveChanges();
        return receivable;
    }

    public ReceivableModel GetReceivableById(string id)
    {
        return _bankAppContext.Receivable.Find(id);
    }

    public ReceivableModel InsertReceivable(ReceivableModel receivable)
    {
        _bankAppContext.Receivable.Add(receivable);
        _bankAppContext.SaveChanges();

        return receivable;
    }

    public ReceivableModel UpdateReceivable(ReceivableModel receivable, ReceivableEditDTO dto)
    {
        receivable.Value = dto.Value ?? receivable.Value;
        receivable.Date = dto.Date ?? receivable.Date;

        _bankAppContext.Update(receivable);
        _bankAppContext.SaveChanges();
        return receivable;
    }
}
