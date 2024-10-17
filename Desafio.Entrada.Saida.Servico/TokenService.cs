using Desafio.entrada.saida.Dominio.Security.Token;
using Desafio.Entrada.Saida.Dominio.DTO;
using Desafio.Entrada.Saida.Dominio.DTO.Requests;
using Desafio.Entrada.Saida.Dominio.DTO.Response;
using Desafio.Entrada.Saida.Dominio.Interfaces;
using Desafio.Entrada.Saida.Helper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Desafio.Entrada.Saida.Servico
{
    public class TokenService : ITokenService
    {
        public async Task<RetornoDto> AutorizarApiKeyToken(string apiKeyToken, string apiKeyUser)
        {
            try
            {

                RetornoDto retornoDto = new RetornoDto();

                var configuracaoSeguranca = ConfiguracoesApplication.ObterConfiguracaoSeguranca();

                if (configuracaoSeguranca.ApiKeyToken != apiKeyToken)
                {
                    retornoDto.HouveErro = true;
                    retornoDto.CodigoErro = "401";
                    retornoDto.MensagemErro = "ApiKey inválida.";
                }
                else if (configuracaoSeguranca.ApiKeyUser != apiKeyUser)
                {
                    retornoDto.HouveErro = true;
                    retornoDto.CodigoErro = "401";
                    retornoDto.MensagemErro = "ApiKeyUser inválida.";
                }
                else
                {
                    retornoDto.HouveErro = false;
                    retornoDto.MensagemErro = "ApiKey validada com sucesso.";
                }

                return retornoDto;

            }
            catch (Exception ex)
            {

                return new RetornoDto(true, "Exception - AutorizarApiKeyToken/User", $"{ex.Message}{Environment.NewLine}", "500", null);
            }

        }
        public async Task<RetornoDto> RetornarTokenAsync(TokenRequest tokenRequisicaoDto, string apiKeyToken, string apiKeyUser, string? ContentType)
        {
            var configToken = ConfiguracoesApplication.ConfiguracaoToken();

            RetornoDto retornoDto = new RetornoDto();

            retornoDto = await ValidarFormTokenAsync(tokenRequisicaoDto, configToken, ContentType);

            if (retornoDto.HouveErro == true)
            {
                return retornoDto;
            }
            else
            {
                try
                {
                    var configJTW = ConfiguracoesApplication.ConfiguracaoJTW();

                    var expireDateByte = System.Text.Encoding.ASCII.GetBytes(DateTime.UtcNow.AddHours(configToken.RefreshTokenExpiresInHours).ToString());
                    var expireDateHash = System.Convert.ToBase64String(expireDateByte);

                    string refreshToken = await GenerateRefreshTokenAsync();

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(configJTW.Key);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Hash, refreshToken),
                            new Claim(ClaimTypes.Expiration, expireDateHash),
                            new Claim(ClaimTypes.Name, apiKeyUser)
                        }),
                        Expires = DateTime.UtcNow.AddHours(configJTW.Expires),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenValido = tokenHandler.WriteToken(token);

                    TimeSpan ts = new TimeSpan(0, Convert.ToInt32(configJTW.Expires), 0);
                    int segundos = int.Parse(ts.TotalSeconds.ToString());

                    TokenResponse integracaoTokenRespostaDto = new TokenResponse();
                    integracaoTokenRespostaDto.Access_token = tokenValido;
                    integracaoTokenRespostaDto.Expires_in = segundos;
                    integracaoTokenRespostaDto.Tokey_type = configToken.Token_type;
                    integracaoTokenRespostaDto.Refresh_token = refreshToken;

                    retornoDto.HouveErro = false;
                    retornoDto.ObjetoRetorno = integracaoTokenRespostaDto;

                    return retornoDto;
                }
                catch (Exception ex)
                {
                    return new RetornoDto(true, "Exception - RetornarToken", $"{ex.Message}{Environment.NewLine}", "500", null);
                }
            }
        }
        public async Task<RetornoDto> RetornarRefreshTokenAsync(RefreshTokenRequest refreshTokenRequisicaoDto, string apiKeyToken, string apiKeyUser)
        {
            RetornoDto retornoDto = new RetornoDto();

            try
            {
                retornoDto = await ValidarExpiredTokenAsync(refreshTokenRequisicaoDto.accessToken);

                if (retornoDto.HouveErro == true)
                {
                    return retornoDto;
                }

                if (retornoDto == null || retornoDto.ObjetoRetorno == null)
                {
                    return new RetornoDto(true, "Erro Refresh Token", new SecurityTokenException("Token ou Refresh Token inválido").ToString(), "400", null);
                }

                var tokenPrincipal = (ClaimsPrincipal)retornoDto.ObjetoRetorno;

                if (tokenPrincipal.Identity == null || tokenPrincipal.Identity.Name == null || tokenPrincipal.Claims == null || tokenPrincipal.Claims.Count() == 0)
                {
                    return new RetornoDto(true, "Erro Refresh Token", new SecurityTokenException("Identificação do Token inválido").ToString(), "400", null);
                }

                var claims = tokenPrincipal.Claims.ToList();

                string ApiTokenUser = tokenPrincipal.Identity.Name;
                string RefreshToken = claims[0].Value;

                var expireDateRefreshTokenByte = System.Convert.FromBase64String(claims[1].Value);
                string expireDateRefreshToken = System.Text.Encoding.ASCII.GetString(expireDateRefreshTokenByte);

                if (string.IsNullOrEmpty(RefreshToken))
                {
                    return new RetornoDto(true, "Erro Refresh Token", new SecurityTokenException("Refresh Token inválido").ToString(), "400", null);
                }

                if (refreshTokenRequisicaoDto.refreshToken != RefreshToken || apiKeyUser != ApiTokenUser || DateTime.Parse(expireDateRefreshToken) < DateTime.Now)
                {
                    return new RetornoDto(true, "Erro Refresh Token", new SecurityTokenException("Refresh Token inválido").ToString(), "400", null);
                }

                var configToken = ConfiguracoesApplication.ConfiguracaoToken();

                TokenRequest tokenRequisicaoDto = new TokenRequest();
                tokenRequisicaoDto.clientId = configToken.Client_id;
                tokenRequisicaoDto.clientSecret = configToken.Client_secret;

                retornoDto = await RetornarTokenAsync(tokenRequisicaoDto, apiKeyToken, apiKeyUser, "application/x-www-form-urlencoded");

                return retornoDto;

            }
            catch (Exception ex)
            {
                return new RetornoDto(true, "Exception - ValidarFormToken", $"{ex.Message}{Environment.NewLine}", "500", null);
            }

        }
        internal async Task<RetornoDto> ValidarFormTokenAsync(TokenRequest tokenRequisicao,
                                                              TokenSettings tokenSettings,
                                                              string? ContentType)
        {
            RetornoDto retornoDto = new RetornoDto();

            try
            {

                if (tokenRequisicao.clientId != tokenSettings.Client_id)
                {
                    return new RetornoDto(true, "Token", $"Client Id inválido", "400", null);
                }

                if (tokenRequisicao.clientSecret != tokenSettings.Client_secret)
                {
                    return new RetornoDto(true, "Token", $"Client Secret inválido", "400", null);
                }

                if (ContentType != tokenSettings.MediaType)
                {
                    return new RetornoDto(true, "Token", $"Media Type inválido", "400", null);
                }

                return retornoDto;
            }
            catch (Exception ex)
            {
                return new RetornoDto(true, "Exception - ValidarFormToken", $"{ex.Message}{Environment.NewLine}", "500", null);
            }

        }
        internal async Task<string> GenerateRefreshTokenAsync()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        internal async Task<RetornoDto> ValidarExpiredTokenAsync(string? token)
        {
            RetornoDto retornoDto = new RetornoDto();

            try
            {
                var configJTW = ConfiguracoesApplication.ConfiguracaoJTW();

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configJTW.Key)),
                    ValidateLifetime = false
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken)
                {
                    return new RetornoDto(true, "Erro refresh token", new SecurityTokenException("Token inválido").ToString(), "400", null);
                }

                if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                {
                    return new RetornoDto(true, "Erro refresh token", new SecurityTokenException("Token inválido").ToString(), "400", null);
                }

                retornoDto.HouveErro = false;
                retornoDto.ObjetoRetorno = principal;

                return retornoDto;
            }
            catch (Exception ex)
            {
                return new RetornoDto(true, "Exception - ValidarExpiredToken", $"{ex.Message}{Environment.NewLine}", "500", null);
            }
        }
    }
}
