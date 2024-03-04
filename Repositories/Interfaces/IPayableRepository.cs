namespace BankProject;

public interface IPayableRepository
{
    public ReceivableModel InsertReceivable(ReceivableModel receivable);
    public ReceivableModel GetReceivableById(string id);
    public ReceivableModel UpdateReceivable(ReceivableModel receivable, ReceivableEditDTO dto);
    public ReceivableModel DeleteReceivable(string id);
}
