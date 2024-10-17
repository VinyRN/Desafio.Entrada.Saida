using Desafio.Entrada.Saida.Dominio.DTO;
using Desafio.Entrada.Saida.Dominio.DTO.Requests;

namespace Desafio.Entrada.Saida.Dominio.Interfaces
{
    public interface ITokenService
    {
        Task<RetornoDto> AutorizarApiKeyToken(string apiKeyToken, string apiKeyUser);
        Task<RetornoDto> RetornarTokenAsync(TokenRequest tokenRequisicaoDto, string apiKeyToken, string apiKeyUser, string? ContentType);
        Task<RetornoDto> RetornarRefreshTokenAsync(RefreshTokenRequest refreshTokenRequisicaoDto, string apiKeyToken, string apiKeyUser);
    }
}
