using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BankProject;

public class CustomValidationProblemDetails : ValidationProblemDetails
{
    [JsonPropertyName("errors")]
    public List<string> Errors { get; } = [];

    public CustomValidationProblemDetails() {
    }
    
    public CustomValidationProblemDetails(List<ErrorMessagesDTO> errors) {
        foreach(ErrorMessagesDTO error in errors) {
            Errors.Add(error.ErrorMessage);
        }
    }

    public CustomValidationProblemDetails(ModelStateDictionary errors) {
        Errors = ConvertModelToList(errors);
    }

    public CustomValidationProblemDetails(Dictionary<string, string[]> errors) {
        Errors = ConvertDictToList(errors);
    }

    private List<string> ConvertModelToList(ModelStateDictionary errorsModelState) {
        List<string> listErrors = new();

        foreach(KeyValuePair<string, ModelStateEntry> keyModelStatePair in errorsModelState) {
            ModelErrorCollection errors = keyModelStatePair.Value.Errors;
            switch(errors.Count) {
                case 0:
                    continue;
                case 1:
                    listErrors.Add(errors[0].ErrorMessage);
                    break;
                default:
                    listErrors.Add(string.Join(Environment.NewLine, errors.Select(x => x.ErrorMessage)));
                    break;
            }
        }

        return listErrors;
    }

    private List<string> ConvertDictToList(Dictionary<string, string[]> errorsDict) {
        List<string> listErrors = new();

        foreach(KeyValuePair<string, string[]> keyAndValue in errorsDict) {
            string[] errors = keyAndValue.Value;
            switch(errors.Length) {
                case 0:
                    continue;
                case 1:
                    listErrors.Add(errors[0]);
                    break;
                default:
                    listErrors.Add(string.Join(Environment.NewLine, errors.Select(x => x)));
                    break;
            }
        }

        return listErrors;
    }
}
