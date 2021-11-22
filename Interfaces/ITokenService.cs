using MinimalApi.Services;
using MinimalApi.Models;

namespace MinimalApi.Interfaces
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, Usuario user);
    }
}
