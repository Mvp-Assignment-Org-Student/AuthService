using Business.Dtos;
using Business.Models;

namespace Business.Services
{
    public interface IAuthService
    {
        Task<AuthServiceResult> SendToAccountService(VerifiedDtoRequest request);
        Task<AuthServiceResult> SignUp(SignUpDto dto);
    }
}