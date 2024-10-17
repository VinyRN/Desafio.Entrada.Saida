using Desafio.Entrada.Saida.Dominio.DTO.Request;

namespace Desafio.entrada.saida.Dominio.DTO
{
    public class EmbalagemResultadoRequest
    {
        public List<PedidoRequest> Pedidos { get; set; }

        public EmbalagemResultadoRequest()
        {
            Pedidos = new List<PedidoRequest>();
        }
    }
}
