using Desafio.Entrada.Saida.Dominio.DTO.Response;

namespace Desafio.entrada.saida.Dominio.DTO
{
    public class EmbalagemResultadoResponse
    {
        public List<PedidoResponse> PedidosProcessados { get; set; }
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;

        public EmbalagemResultadoResponse()
        {
            PedidosProcessados = new List<PedidoResponse>();
        }
    }
}
