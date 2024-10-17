namespace Desafio.entrada.saida.Dominio.Security.Token
{
    public  class TokenSettings
    {
        public string Token_type { get; set; }
        public string Scope { get; set; }
        public string Grant_type { get; set; }
        public string Client_id { get; set; }
        public string Client_secret { get; set; }
        public string MediaType { get; set; }
        public int RefreshTokenExpiresInHours { get; set; }
    }
}
