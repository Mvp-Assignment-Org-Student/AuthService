using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Business.Services;

public class AuthService(IMemoryCache cache) : IAuthService
{

    private readonly IMemoryCache _cache = cache;

    string AccountServiceUrl = "https://accountservice-brcpcveraagah0cd.swedencentral-01.azurewebsites.net/";
    string VerificationServiceUrl = "https://verificationservice-ercbhacafnhac8gj.swedencentral-01.azurewebsites.net/";
    
    public async Task<AuthServiceResult> ExistsAsync(string email)
    {

        using var http = new HttpClient();

        // Hjälp av chatgpt 
        var request = new { Email = email };

        var response = await http.PostAsJsonAsync($"{AccountServiceUrl}/api/Account/exists/email", request);
        
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
        if (!exist.Success)
        {
            return new AuthServiceResult { Success = false };
        }

        // Postar email till send verify kod
        using var http = new HttpClient();


        var createUserResponse = await http.PostAsJsonAsync($"{AccountServiceUrl}/api/Account/create", dto);
        if (!createUserResponse.IsSuccessStatusCode)
        {
            return new AuthServiceResult { Success = false, Error = "Failed to create user" };

        }

        var request = new { Email = dto.Email };


        var verifyCodeResponse = await http.PostAsJsonAsync($"{VerificationServiceUrl}/api/Verification/send", request);
       
        if (!verifyCodeResponse.IsSuccessStatusCode)
        {
            return new AuthServiceResult { Success = false };
        }

        return new AuthServiceResult { Success = true };
        // Sedan till frontend delen

    }

    public async Task<AuthServiceResult> SignIn(SignInDto dto)
    {
        using var http = new HttpClient();

        var response = await http.PostAsJsonAsync($"{AccountServiceUrl}/api/Account/login", dto);

        if (!response.IsSuccessStatusCode)
        {
            return new AuthServiceResult { Success = false, Error = "Login failed" };
        }

        var content = await response.Content.ReadFromJsonAsync<AccountServiceResultToken>();

        return new AuthServiceResult
        {
            Success = true,
            Token = content?.Token
        };
    }

    public async Task<AuthServiceResult> SignOut()
    {
        using var http = new HttpClient();

        var response = await http.PostAsync($"{AccountServiceUrl}/api/Account/logout", null);

        if (!response.IsSuccessStatusCode)
        {
            return new AuthServiceResult { Success = false, Error = "Logout failed" };
        }

        return new AuthServiceResult { Success = true };
    }


    public async Task<AuthServiceResult> VerifyCodeAndConfirmEmail(VerifiedDtoRequest request)
    {
        using var http = new HttpClient();

        var verifyRequest = new VerifyVerificationCodeRequest
        {
            Email = request.Email,
            Code = request.Code
        };  

        var verifyCodeResponse = await http.PostAsJsonAsync($"{VerificationServiceUrl}/api/Verification/verify", verifyRequest);

        if (!verifyCodeResponse.IsSuccessStatusCode)
        {
            return new AuthServiceResult { Success = false, Error = "Code is invalid" };
        }

        var confirmEmailResponse = await http.PostAsJsonAsync($"{AccountServiceUrl}/api/Account/confirm-email", new ConfirmEmailRequest { Email = verifyRequest.Email });

        if (!confirmEmailResponse.IsSuccessStatusCode)
        {
            return new AuthServiceResult { Success = false, Error = "Failed to confirm email" };

        }

        return new AuthServiceResult { Success = true };
    }

    

}
