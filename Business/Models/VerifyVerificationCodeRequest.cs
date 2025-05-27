namespace Business.Models;

public class VerifyVerificationCodeRequest
{
    public string Email { get; set; } = null!;
    public string VerifyCode { get; set; } = null!;
}
