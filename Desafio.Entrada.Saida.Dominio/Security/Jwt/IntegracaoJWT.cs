namespace Desafio.entrada.saida.Dominio.Security.Jwt
{
    public class IntegracaoJWT
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int Expires { get; set; }
    }
}
