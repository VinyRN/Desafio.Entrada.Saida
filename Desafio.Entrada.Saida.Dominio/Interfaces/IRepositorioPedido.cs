using Desafio.Entrada.Saida.Dominio.DTO.Request;
using Desafio.Entrada.Saida.Dominio.DTO.Response;

namespace Desafio.Entrada.Saida.Dominio.Interfaces
{
    /// <summary>
    /// Interface responsável pelas operações de acesso aos dados dos pedidos.
    /// </summary>
    public interface IRepositorioPedido
    {
        /// <summary>
        /// Adiciona um novo pedido.
        /// </summary>
        /// <param name="pedido">O pedido a ser adicionado.</param>
        void AdicionarPedido(PedidoRequest pedido);

        /// <summary>
        /// Obtém um pedido pelo seu identificador.
        /// </summary>
        /// <param name="idPedido">O identificador do pedido.</param>
        /// <returns>O pedido correspondente ao identificador.</returns>
        PedidoResponse ObterPedidoPorId(int idPedido);

        /// <summary>
        /// Obtém todos os pedidos cadastrados.
        /// </summary>
        /// <returns>Uma lista de todos os pedidos.</returns>
        IEnumerable<PedidoResponse> ObterTodosPedidos();

        /// <summary>
        /// Atualiza as informações de um pedido existente.
        /// </summary>
        /// <param name="pedido">O pedido com as informações atualizadas.</param>
        void AtualizarPedido(PedidoRequest pedido);

        /// <summary>
        /// Remove um pedido com base no seu identificador.
        /// </summary>
        /// <param name="idPedido">O identificador do pedido a ser removido.</param>
        void RemoverPedido(int idPedido);
    }
}
