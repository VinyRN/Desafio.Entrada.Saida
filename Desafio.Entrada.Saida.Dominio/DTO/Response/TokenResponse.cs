namespace Desafio.Entrada.Saida.Dominio.DTO.Response
{
    public class TokenResponse
    {
        public string Access_token { get; set; }
        public string Tokey_type { get; set; }
        public int Expires_in { get; set; }
        public string Refresh_token { get; set; }
    }
}
