using Desafio.entrada.saida.Dominio.DTO;
using Desafio.entrada.saida.Dominio;
using Desafio.Entrada.Saida.Dominio;
using Desafio.Entrada.Saida.Dominio.DTO.Request;
using Desafio.Entrada.Saida.Dominio.DTO.Requests;
using Desafio.Entrada.Saida.Dominio.DTO.Response;
using Desafio.Entrada.Saida.Queue;
using Desafio.Entrada.Saida.Queue.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Desafio.Entrada.Saida.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmbalagemController : ControllerBase
    {
        private readonly IEmbalagemService _embalagemService;
        private readonly IQueueService _queueService;
        private readonly Serilog.ILogger _logger;

        public EmbalagemController(IEmbalagemService embalagemService, IQueueService queueService)
        {
            _embalagemService = embalagemService;
            _queueService = queueService;
            _logger = Log.ForContext<EmbalagemController>();
        }

        [HttpPost("processar-pedidos")]
        public ActionResult<EmbalagemResultadoResponse> ProcessarPedidos([FromBody] List<PedidoRequest> pedidos)
        {
            _logger.Information("Recebendo requisição para processar {Count} pedidos.", pedidos?.Count);

            if (pedidos == null || pedidos.Count == 0)
            {
                _logger.Warning("A lista de pedidos está vazia ou é nula.");
                return BadRequest("A lista de pedidos não pode estar vazia.");
            }

            try
            {
                var pedidosJson = System.Text.Json.JsonSerializer.Serialize(pedidos);
                var resultado = _embalagemService.ProcessarPedidos(pedidosJson);

                if (!resultado.Sucesso)
                {
                    _logger.Error("Erro ao processar os pedidos: {Mensagem}", resultado.Mensagem);
                    return BadRequest(resultado.Mensagem);
                }

                // Publica uma mensagem no RabbitMQ
                _queueService.Publish("processamento_pedidos", pedidosJson);

                _logger.Information("Pedidos processados com sucesso.");
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Ocorreu um erro ao processar os pedidos.");
                return StatusCode(500, "Erro interno ao processar os pedidos.");
            }
        }
    }
}
