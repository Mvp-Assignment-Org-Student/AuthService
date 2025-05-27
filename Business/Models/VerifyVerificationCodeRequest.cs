namespace Business.Models;

public class VerifyVerificationCodeRequest
{
    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;
}
