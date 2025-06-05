using Business.Dtos;
using Business.Models;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<AuthServiceResult> SignIn(SignInDto dto);
        Task<AuthServiceResult> SignOut();
        Task<AuthServiceResult> SignUp(SignUpDto dto);
        Task<AuthServiceResult> VerifyCodeAndConfirmEmail(VerifiedDtoRequest request);
    }
}