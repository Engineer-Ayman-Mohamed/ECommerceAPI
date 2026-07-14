namespace ECommerceAPI.Application.Shared.Errors;

public class ApiValidationError
{
    public int Status { get; set; } = 400;
    public string Title { get; set; } = "Validation Error";
    public Dictionary<string, List<string>> Errors { get; set; } = new();
}
