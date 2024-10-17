namespace Desafio.Entrada.Saida.Queue.Interfaces
{
    /// <summary>
    /// Interface para o serviço de consumo de mensagens da fila.
    /// </summary>
    public interface IQueueConsumer
    {
        /// <summary>
        /// Inicia o consumo de mensagens da fila especificada.
        /// </summary>
        /// <param name="queueName">Nome da fila.</param>
        void Consume(string queueName);
    }
}
