using Desafio.Entrada.Saida.Dominio.DTO.Requests;
using Desafio.Entrada.Saida.Dominio.DTO.Response;
using Desafio.Entrada.Saida.Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace Desafio.Entrada.Saida.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public AuthorizeController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]

        public IActionResult HealthCheck()
        {
            StringBuilder informacoes = new StringBuilder();
            informacoes.AppendLine($"API Autenicação");
            informacoes.AppendLine($"Situação = Saudável");

            return Ok(informacoes.ToString());
        }


        [HttpPost]
        [Route("/Token")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> TokenAsync([Required(ErrorMessage = "apiKey(Header) Obrigatório.; "), FromHeader(Name = "apiKey")] string apiKey,
                                                    [Required(ErrorMessage = "apiKeyUser(Header) Obrigatório.; "), FromHeader(Name = "apiKeyUser")] string apiKeyUser,
                                                    [FromForm] TokenRequest tokenRequisicaoDto)
        {
            if (tokenRequisicaoDto != null)
            {

                var ApyKeyretornoDto = await _tokenService.AutorizarApiKeyToken(apiKey, apiKeyUser);

                if (ApyKeyretornoDto.HouveErro == true)
                {
                    return ApyKeyretornoDto.RetornarResultado(HttpContext.Request.Path);
                }

                string? ContentType = HttpContext.Request.ContentType;

                var retornoDto = await _tokenService.RetornarTokenAsync(tokenRequisicaoDto, apiKey, apiKeyUser, ContentType);

                if (retornoDto.HouveErro == false)
                {
                    var tokenRespostaDto = (TokenResponse)retornoDto.ObjetoRetorno;
                    return StatusCode((int)HttpStatusCode.OK, tokenRespostaDto);
                }
                else
                {
                    return retornoDto.RetornarResultado(HttpContext.Request.Path);
                }

                return null;
            }
            else
            {
                ProblemDetails detalhesDoProblema = new ProblemDetails();
                detalhesDoProblema.Status = StatusCodes.Status400BadRequest;
                detalhesDoProblema.Type = "BadRequest";
                detalhesDoProblema.Title = "Registro não pode ser nulo";
                detalhesDoProblema.Detail = $"Dados não podem ser vazio ou nulo. ";
                detalhesDoProblema.Instance = HttpContext.Request.Path;
                return BadRequest(detalhesDoProblema);
            }

        }
    }
}
