using Desafio.Entrada.Saida.Dominio.DTO.Requests;
using Desafio.Entrada.Saida.Dominio.DTO.Response;

namespace Desafio.Entrada.Saida.Dominio.Interfaces
{
    /// <summary>
    /// Interface responsável pelas operações de acesso aos dados das caixas.
    /// </summary>
    public interface IRepositorioCaixa
    {
        /// <summary>
        /// Adiciona uma nova caixa.
        /// </summary>
        /// <param name="caixa">A caixa a ser adicionada.</param>
        void AdicionarCaixa(CaixaRequest caixa);

        /// <summary>
        /// Obtém uma caixa pelo seu identificador.
        /// </summary>
        /// <param name="idCaixa">O identificador da caixa.</param>
        /// <returns>A caixa correspondente ao identificador.</returns>
        CaixaResponse ObterCaixaPorId(int idCaixa);

        /// <summary>
        /// Obtém todas as caixas cadastradas.
        /// </summary>
        /// <returns>Uma lista de todas as caixas.</returns>
        IEnumerable<CaixaResponse> ObterTodasCaixas();

        /// <summary>
        /// Atualiza as informações de uma caixa existente.
        /// </summary>
        /// <param name="caixa">A caixa com as informações atualizadas.</param>
        void AtualizarCaixa(CaixaRequest caixa);

        /// <summary>
        /// Remove uma caixa com base no seu identificador.
        /// </summary>
        /// <param name="idCaixa">O identificador da caixa a ser removida.</param>
        void RemoverCaixa(int idCaixa);
    }
}
