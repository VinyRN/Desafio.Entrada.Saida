namespace Desafio.Entrada.Saida.Dominio.DTO.Response
{
    public class CaixaResponse
    {
        public int IdCaixa { get; set; }
        public decimal Altura { get; set; }
        public decimal Largura { get; set; }
        public decimal Comprimento { get; set; }
        public decimal Capacidade { get; set; }
        public List<int> IdsProdutos { get; set; }

        public CaixaResponse()
        {
            IdsProdutos = new List<int>();
        }
    }
}
