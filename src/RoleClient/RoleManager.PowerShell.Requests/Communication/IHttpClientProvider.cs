namespace RoleManager.PowerShell.Requests.Communication;

public interface IHttpClientProvider 
{
    HttpClient Get();
}