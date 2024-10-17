using Desafio.entrada.saida.Dominio.Security.Jwt;
using Desafio.entrada.saida.Dominio.Security.Settings;
using Desafio.entrada.saida.Dominio.Security.Token;

namespace Desafio.Entrada.Saida.Helper
{
    public class ConfiguracoesApplication
    {

        public static IntegracaoJWT ConfiguracaoJTW()
        {
            Configuracao configuracao = new Configuracao();

            IntegracaoJWT integracaoJWT = new();
            integracaoJWT.Issuer = configuracao.ConfiguracaoAppSettings["Jwt:Issuer"];
            integracaoJWT.Audience = configuracao.ConfiguracaoAppSettings["Jwt:Audience"];
            integracaoJWT.Key = configuracao.ConfiguracaoAppSettings["Jwt:Key"];
            integracaoJWT.Expires = int.Parse(configuracao.ConfiguracaoAppSettings["Jwt:Expires"]);

            return integracaoJWT;
        }

        public static TokenSettings ConfiguracaoToken()
        {
            Configuracao configuracao = new Configuracao();

            TokenSettings tokenSettings = new();
            tokenSettings.Token_type = configuracao.ConfiguracaoAppSettings["TokenSettings:Token_type"];
            tokenSettings.Scope = configuracao.ConfiguracaoAppSettings["TokenSettings:Scope"];
            tokenSettings.Grant_type = configuracao.ConfiguracaoAppSettings["TokenSettings:Grant_type"];
            tokenSettings.Client_id = configuracao.ConfiguracaoAppSettings["TokenSettings:Client_id"];
            tokenSettings.Client_secret = configuracao.ConfiguracaoAppSettings["TokenSettings:Client_secret"];
            tokenSettings.MediaType = configuracao.ConfiguracaoAppSettings["TokenSettings:MediaType"];
            tokenSettings.RefreshTokenExpiresInHours = int.Parse(configuracao.ConfiguracaoAppSettings["TokenSettings:RefreshTokenExpiresInHours"]);

            return tokenSettings;
        }


        public static SecuritySetting ObterConfiguracaoSeguranca()
        {
            Configuracao configuracao = new Configuracao();

            SecuritySetting securitySettings = new();

            securitySettings.ApiKeyToken = configuracao.ConfiguracaoAppSettings["SecuritySettings:ApiKeyToken"];
            securitySettings.ApiKeyUser = configuracao.ConfiguracaoAppSettings["SecuritySettings:ApiKeyUser"];

            return securitySettings;
        }


    }
}
