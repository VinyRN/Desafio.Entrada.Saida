namespace Desafio.Entrada.Saida.Queue.Interfaces
{
    /// <summary>
    /// Interface para o serviço de fila para envio de mensagens.
    /// </summary>
    public interface IQueueService
    {
        /// <summary>
        /// Publica uma mensagem na fila especificada.
        /// </summary>
        /// <param name="queueName">Nome da fila.</param>
        /// <param name="message">Mensagem a ser publicada.</param>
        void Publish(string queueName, string message);
    }
}
