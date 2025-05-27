using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class SignUpDto
{
    [Required]
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
