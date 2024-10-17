namespace Desafio.Entrada.Saida.Dominio.DTO.Request
{
    public class PedidoRequest
    {
        public int IdPedido { get; set; }
        public List<ProdutoRequest> Produtos { get; set; }

        public PedidoRequest()
        {
            Produtos = new List<ProdutoRequest>();
        }
    }
}
