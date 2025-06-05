
namespace Business.Models;

public class AccountServiceResultToken
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public string? Message { get; set; }
    public string? Token { get; set; } // Lägg till Token här
}
