using Desafio.Entrada.Saida.Dominio.DTO.Requests;
using Desafio.Entrada.Saida.Dominio.DTO.Response;
using Desafio.Entrada.Saida.Dominio.Interfaces;

namespace Desafio.Entrada.Saida.Infraestrutura.Repositorios.Repository
{
    /// <summary>
    /// Repositório responsável pelas operações de acesso aos dados das caixas.
    /// </summary>
    public class RepositorioCaixa : IRepositorioCaixa
    {
        private readonly List<CaixaResponse> _caixas;

        /// <summary>
        /// Inicializa uma nova instância do repositório de caixas com as caixas pré-fabricadas.
        /// </summary>
        public RepositorioCaixa()
        {
            _caixas = new List<CaixaResponse>
            {
                new CaixaResponse { IdCaixa = 1, Altura = 30, Largura = 40, Comprimento = 80, IdsProdutos = new List<int>() },
                new CaixaResponse { IdCaixa = 2, Altura = 80, Largura = 50, Comprimento = 40, IdsProdutos = new List<int>() },
                new CaixaResponse { IdCaixa = 3, Altura = 50, Largura = 80, Comprimento = 60, IdsProdutos = new List<int>() }
            };
        }

        /// <summary>
        /// Adiciona uma nova caixa.
        /// </summary>
        /// <param name="caixa">A caixa a ser adicionada.</param>
        public void AdicionarCaixa(CaixaRequest caixa)
        {
            var novaCaixa = new CaixaResponse
            {
                IdCaixa = _caixas.Count > 0 ? _caixas.Max(c => c.IdCaixa) + 1 : 1,
                Altura = caixa.Altura,
                Largura = caixa.Largura,
                Comprimento = caixa.Comprimento,
                Capacidade = caixa.Capacidade,
                IdsProdutos = new List<int>()
            };

            _caixas.Add(novaCaixa);
        }

        /// <summary>
        /// Obtém uma caixa pelo seu identificador.
        /// </summary>
        /// <param name="idCaixa">O identificador da caixa.</param>
        /// <returns>A caixa correspondente ao identificador ou null se não for encontrada.</returns>
        public CaixaResponse ObterCaixaPorId(int idCaixa)
        {
            var caixa = _caixas.FirstOrDefault(c => c.IdCaixa == idCaixa);

            if (caixa == null)
            {
                // Aqui você pode registrar um log, se necessário, informando que a caixa não foi encontrada
                caixa = new CaixaResponse();
                Console.WriteLine($"Nenhuma caixa encontrada com o identificador: {idCaixa}.");
            }

            return caixa;
        }

        /// <summary>
        /// Obtém todas as caixas cadastradas.
        /// </summary>
        /// <returns>Uma lista de todas as caixas.</returns>
        public IEnumerable<CaixaResponse> ObterTodasCaixas()
        {
            return _caixas;
        }

        /// <summary>
        /// Atualiza as informações de uma caixa existente.
        /// </summary>
        /// <param name="caixa">A caixa com as informações atualizadas.</param>
        public void AtualizarCaixa(CaixaRequest caixa)
        {
            var caixaExistente = ObterCaixaPorId(caixa.IdCaixa);
            if (caixaExistente != null)
            {
                caixaExistente.Altura = caixa.Altura;
                caixaExistente.Largura = caixa.Largura;
                caixaExistente.Comprimento = caixa.Comprimento;
                caixaExistente.Capacidade = caixa.Capacidade;
                // Mantém os produtos existentes ou atualiza conforme necessário
            }
        }

        /// <summary>
        /// Remove uma caixa com base no seu identificador.
        /// </summary>
        /// <param name="idCaixa">O identificador da caixa a ser removida.</param>
        public void RemoverCaixa(int idCaixa)
        {
            var caixa = ObterCaixaPorId(idCaixa);
            if (caixa != null)
            {
                _caixas.Remove(caixa);
            }
        }

        /// <summary>
        /// Adiciona um produto a uma caixa específica.
        /// </summary>
        /// <param name="idCaixa">O identificador da caixa.</param>
        /// <param name="idProduto">O identificador do produto a ser adicionado.</param>
        public void AdicionarProdutoNaCaixa(int idCaixa, int idProduto)
        {
            var caixa = ObterCaixaPorId(idCaixa);
            if (caixa != null && !caixa.IdsProdutos.Contains(idProduto))
            {
                caixa.IdsProdutos.Add(idProduto);
            }
        }

        /// <summary>
        /// Remove um produto de uma caixa específica.
        /// </summary>
        /// <param name="idCaixa">O identificador da caixa.</param>
        /// <param name="idProduto">O identificador do produto a ser removido.</param>
        public void RemoverProdutoDaCaixa(int idCaixa, int idProduto)
        {
            var caixa = ObterCaixaPorId(idCaixa);
            if (caixa != null && caixa.IdsProdutos.Contains(idProduto))
            {
                caixa.IdsProdutos.Remove(idProduto);
            }
        }
    }
}
