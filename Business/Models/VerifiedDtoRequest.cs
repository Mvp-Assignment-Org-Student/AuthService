using Business.Dtos;

namespace Business.Models;

public class VerifiedDtoRequest
{
    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;
}
