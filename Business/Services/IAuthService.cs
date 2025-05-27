using Business.Dtos;
using Business.Models;

namespace Business.Services
{
    public interface IAuthService
    {
        Task<AuthServiceResult> SignUp(SignUpDto dto);
        Task<AuthServiceResult> VerifyCodeAndConfirmEmail(VerifiedDtoRequest request);
    }
}