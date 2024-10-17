using Desafio.Entrada.Saida.Dominio.DTO.Requests;
using Desafio.entrada.saida.Dominio.DTO;
using Desafio.entrada.saida.Dominio;
using Desafio.Entrada.Saida.Dominio.DTO.Response;
using Desafio.Entrada.Saida.Dominio.Interfaces;
using Newtonsoft.Json;
using Desafio.Entrada.Saida.Dominio.DTO.Request;

namespace Desafio.Entrada.Saida.Servico
{
    /// <summary>
    /// Serviço responsável pelo processamento e embalagem de pedidos.
    /// </summary>
    public class EmbalagemService : IEmbalagemService
    {
        private readonly IRepositorioCaixa _repositorioCaixa;

        /// <summary>
        /// Inicializa uma nova instância do serviço de embalagem com o repositório necessário.
        /// </summary>
        /// <param name="repositorioCaixa">Repositório de caixas.</param>
        public EmbalagemService(IRepositorioCaixa repositorioCaixa)
        {
            _repositorioCaixa = repositorioCaixa;
        }

        /// <summary>
        /// Processa uma lista de pedidos, determinando a melhor forma de embalar os produtos.
        /// </summary>
        /// <param name="pedidosJson">O JSON contendo a lista de pedidos.</param>
        /// <returns>Um objeto que representa a solução de embalagem para cada pedido, incluindo as caixas selecionadas e os produtos contidos em cada uma.</returns>
        public EmbalagemResultadoResponse ProcessarPedidos(string pedidosJson)
        {
            // Desserializar o JSON para uma lista de PedidoRequest
            var pedidos = JsonConvert.DeserializeObject<List<PedidoRequest>>(pedidosJson);
            if (pedidos == null)
            {
                pedidos = new List<PedidoRequest>();
            }

            var resultado = new EmbalagemResultadoResponse
            {
                PedidosProcessados = new List<PedidoResponse>(),
                Sucesso = true
            };

            foreach (var pedido in pedidos)
            {
                var pedidoResponse = new PedidoResponse
                {
                    IdPedido = pedido.IdPedido,
                    CaixasUtilizadas = new List<CaixaResponse>()
                };

                foreach (var produto in pedido.Produtos)
                {
                    var caixa = EncontrarCaixaApropriada(produto);
                    if (caixa != null)
                    {
                        // Atualiza a lista de produtos na caixa
                        if (!caixa.IdsProdutos.Contains(produto.IdProduto))
                        {
                            caixa.IdsProdutos.Add(produto.IdProduto);
                        }

                        // Atualiza o repositório com a caixa modificada
                        var caixaRequest = new CaixaRequest
                        {
                            IdCaixa = caixa.IdCaixa,
                            Altura = caixa.Altura,
                            Largura = caixa.Largura,
                            Comprimento = caixa.Comprimento,
                            Capacidade = caixa.Capacidade
                        };
                        _repositorioCaixa.AtualizarCaixa(caixaRequest);

                        if (!pedidoResponse.CaixasUtilizadas.Exists(c => c.IdCaixa == caixa.IdCaixa))
                        {
                            pedidoResponse.CaixasUtilizadas.Add(caixa);
                        }
                    }
                    else
                    {
                        resultado.Sucesso = false;
                        resultado.Mensagem = $"Não foi possível embalar o produto {produto.IdProduto} do pedido {pedido.IdPedido}.";
                        return resultado;
                    }
                }
                if(pedidoResponse is not null)
                    resultado.PedidosProcessados.Add(pedidoResponse);
            }

            return resultado;
        }


        /// <summary>
        /// Valida as dimensões dos produtos e verifica se é possível embalar os pedidos com as caixas disponíveis.
        /// </summary>
        /// <param name="pedido">Um objeto que representa o pedido com a lista de produtos e suas dimensões.</param>
        /// <returns>Verdadeiro se o pedido puder ser processado, falso caso contrário.</returns>
        public bool ValidarPedido(PedidoRequest pedido)
        {
            foreach (var produto in pedido.Produtos)
            {
                var caixa = EncontrarCaixaApropriada(produto);
                if (caixa == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Retorna a lista de caixas disponíveis para embalar os produtos.
        /// </summary>
        /// <returns>Uma lista de objetos que representam as dimensões das caixas disponíveis.</returns>
        public List<CaixaResponse> ObterCaixasDisponiveis()
        {
            return _repositorioCaixa.ObterTodasCaixas().ToList();
        }

        /// <summary>
        /// Encontra a caixa apropriada para o produto, com base nas dimensões do produto.
        /// </summary>
        /// <param name="produto">O produto a ser embalado.</param>
        /// <returns>A caixa adequada ou null se nenhuma for encontrada.</returns>
        private CaixaResponse EncontrarCaixaApropriada(ProdutoRequest produto)
        {
            // Obtém todas as caixas disponíveis e encontra a que melhor se adapta ao produto
            foreach (var caixa in _repositorioCaixa.ObterTodasCaixas())
            {
                if (produto.Altura <= caixa.Altura &&
                    produto.Largura <= caixa.Largura &&
                    produto.Comprimento <= caixa.Comprimento)
                {
                    return caixa;
                }
            }

            // Retorna null se nenhuma caixa adequada for encontrada
            return new CaixaResponse();
        }
    }
}
