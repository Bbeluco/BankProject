
using System.Reflection;
using System.Xml.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.ObjectPool;
using Microsoft.VisualBasic;

namespace BankProject;


public class PayableService : IPayableService
{
    public async Task<IResult> Payable(PayableDTO dto)
    {
        // string nullItems = CheckIfAnyFieldIsNull(dto);
        // if(nullItems != ""){
        //     return Results.BadRequest(new ErrorMessagesDTO { ErrorMessage = "The following parameters where null: " + nullItems });
        // }

        // Guid guidOutput = new Guid();
        // bool isValid = Guid.TryParse(dto.Receivable.Id, out guidOutput);
        // if(!isValid) {

        //     return Results.BadRequest(new ErrorMessagesDTO { ErrorMessage = "receivable.id is not an UUID" });
        // }

        return Results.Ok(dto);
    }

    private string CheckIfAnyFieldIsNull(PayableDTO dto) {
        string[] result = [];
        PropertyInfo[] propsReceivable = dto.Receivable.GetType().GetProperties();
        PropertyInfo[] propsAssignor = dto.Receivable.GetType().GetProperties();

        foreach (var item in propsReceivable)
        {
            System.Console.WriteLine("item: " + item);
            if(item == null) {
                result.Append(item.ToString().ToLower());
            }
        }

        foreach (var item in propsAssignor)
        {
            if(item == null) {
                result.Append(item.ToString().ToLower());
            }
        }
        return string.Join(", ", result);
    }
}
