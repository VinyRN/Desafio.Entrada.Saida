using Desafio.Entrada.Saida.Queue.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace Desafio.Entrada.Saida.Queue
{
    /// <summary>
    /// Implementação do serviço de fila usando RabbitMQ.
    /// </summary>
    public class QueueService : IQueueService
    {
        private readonly ConnectionFactory _connectionFactory;

        /// <summary>
        /// Inicializa uma nova instância do serviço de fila.
        /// </summary>
        public QueueService()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = "localhost", // Altere conforme necessário
                UserName = "guest",
                Password = "guest"
            };
        }

        /// <summary>
        /// Publica uma mensagem na fila especificada.
        /// </summary>
        /// <param name="queueName">Nome da fila.</param>
        /// <param name="message">Mensagem a ser publicada.</param>
        public void Publish(string queueName, string message)
        {
            using var connection = _connectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($"[x] Enviado: {message} para a fila {queueName}");
        }
    }
}
