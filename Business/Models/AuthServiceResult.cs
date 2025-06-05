namespace Business.Models;

public class AuthServiceResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? Error { get; set; }
    
    public string? Token { get; set; }
}


public class AuthServiceResult<T> : AuthServiceResult
{
    public T? Result { get; set; }
}
