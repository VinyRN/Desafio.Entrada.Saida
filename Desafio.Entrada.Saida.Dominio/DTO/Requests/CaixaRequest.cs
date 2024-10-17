namespace Desafio.Entrada.Saida.Dominio.DTO.Requests
{
    /// <summary>
    /// Representa a solicitação para criar ou atualizar uma caixa.
    /// </summary>
    public class CaixaRequest
    {
        /// <summary>
        /// Identificador único da caixa.
        /// </summary>
        public int IdCaixa { get; set; }

        /// <summary>
        /// Altura da caixa em centímetros.
        /// </summary>
        public decimal Altura { get; set; }

        /// <summary>
        /// Largura da caixa em centímetros.
        /// </summary>
        public decimal Largura { get; set; }

        /// <summary>
        /// Comprimento da caixa em centímetros.
        /// </summary>
        public decimal Comprimento { get; set; }

        /// <summary>
        /// Capacidade máxima da caixa em volume (cm³).
        /// </summary>
        public decimal Capacidade { get; set; }
    }
}
