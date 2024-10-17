namespace Desafio.Entrada.Saida.Dominio.DTO.Requests
{
    public class RefreshTokenRequest
    {
        public string? accessToken { get; set; }
        public string? refreshToken { get; set; }
    }
}
