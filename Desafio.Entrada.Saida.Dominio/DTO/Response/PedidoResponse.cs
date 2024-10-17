namespace Desafio.Entrada.Saida.Dominio.DTO.Response
{
    public class PedidoResponse
    {
        public int IdPedido { get; set; }
        public List<CaixaResponse> CaixasUtilizadas { get; set; }

        public PedidoResponse()
        {
            CaixasUtilizadas = new List<CaixaResponse>();
        }
    }
}
