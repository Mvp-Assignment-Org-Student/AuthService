using Business.Dtos;
using Business.Models;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Services;

public class AuthService(IMemoryCache cache) : IAuthService
{

    private readonly IMemoryCache _cache = cache;

    public async Task<AuthServiceResult> ExistsAsync(string email)
    {

        using var http = new HttpClient();

        // Hjälp av chatgpt 
        var request = new { Email = email };

        var response = await http.PostAsJsonAsync($"https://accountservice-brcpcveraagah0cd.swedencentral-01.azurewebsites.net/api/Account/exists/email", request);
        
        if (!response.IsSuccessStatusCode)
        {
            return new AuthServiceResult { Success = false };
        }
        else
        {
            return new AuthServiceResult { Success = true};
        }
    }

    public async Task<AuthServiceResult> SignUp(SignUpDto dto)
    {
        var exist = await ExistsAsync(dto.Email);
        if (exist.Success)
        {
            return new AuthServiceResult { Success = false };
        }

        // Postar email till send verify kod
        using var http = new HttpClient();


        var createUserResponse = await http.PostAsJsonAsync("https://accountservice-brcpcveraagah0cd.swedencentral-01.azurewebsites.net/api/Account/create", dto);
        if (!createUserResponse.IsSuccessStatusCode)
        {
            return new AuthServiceResult { Success = false, Error = "Failed to create user" };

        }

        var request = new { Email = dto.Email };


        var verifyCodeResponse = await http.PostAsJsonAsync("https://verificationservice-ercbhacafnhac8gj.swedencentral-01.azurewebsites.net/api/Verification/send", request);
       
        if (!verifyCodeResponse.IsSuccessStatusCode)
        {
            return new AuthServiceResult { Success = false };
        }

        return new AuthServiceResult { Success = true };
        // Sedan till frontend delen

    }
    // Ska få in Email från frontend på något sätt
    public async Task<AuthServiceResult> VerifyCodeAndConfirmEmail(VerifiedDtoRequest request)
    {
        using var http = new HttpClient();

        var verifyRequest = new VerifyVerificationCodeRequest
        {
            Email = request.Email,
            Code = request.Code
        };  

        var verifyCodeResponse = await http.PostAsJsonAsync("https://verificationservice-ercbhacafnhac8gj.swedencentral-01.azurewebsites.net/api/Verification/verify", verifyRequest);

        if (!verifyCodeResponse.IsSuccessStatusCode)
        {
            return new AuthServiceResult { Success = false, Error = "Code is invalid" };
        }

        var confirmEmailResponse = await http.PostAsJsonAsync("https://accountservice-brcpcveraagah0cd.swedencentral-01.azurewebsites.net/api/Account/confirm-email", new ConfirmEmailRequest { Email = verifyRequest.Email });

        if (!confirmEmailResponse.IsSuccessStatusCode)
        {
            return new AuthServiceResult { Success = false, Error = "Failed to confirm email" };

        }

        return new AuthServiceResult { Success = true };
    }


    
}
