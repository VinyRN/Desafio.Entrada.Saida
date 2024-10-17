using Desafio.entrada.saida.Dominio.DTO;
using Desafio.Entrada.Saida.Dominio.DTO.Request;
using Desafio.Entrada.Saida.Dominio.DTO.Requests;
using Desafio.Entrada.Saida.Dominio.DTO.Response;

namespace Desafio.entrada.saida.Dominio
{
    public interface IEmbalagemService
    {
        /// <summary>
        /// Processa uma lista de pedidos, determinando a melhor forma de embalar os produtos.
        /// </summary>
        /// <param name="pedidosJson">O JSON contendo a lista de pedidos.</param>
        /// <returns>Um objeto que representa a solução de embalagem para cada pedido, incluindo as caixas selecionadas e os produtos contidos em cada uma.</returns>
        EmbalagemResultadoResponse ProcessarPedidos(string pedidosJson);

        /// <summary>
        /// Valida as dimensões dos produtos e verifica se é possível embalar os pedidos com as caixas disponíveis.
        /// </summary>
        /// <param name="pedido">Um objeto que representa o pedido com a lista de produtos e suas dimensões.</param>
        /// <returns>Verdadeiro se o pedido puder ser processado, falso caso contrário.</returns>
        bool ValidarPedido(PedidoRequest pedido);

        /// <summary>
        /// Retorna a lista de caixas disponíveis para embalar os produtos.
        /// </summary>
        /// <returns>Uma lista de objetos que representam as dimensões das caixas disponíveis.</returns>
        List<CaixaResponse> ObterCaixasDisponiveis();
    }
}
