using Desafio.Entrada.Saida.Queue.Interfaces;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace Desafio.Entrada.Saida.Queue
{
    /// <summary>
    /// Implementação do serviço de consumo de mensagens da fila usando RabbitMQ.
    /// </summary>
    public class QueueConsumer : IQueueConsumer
    {
        private readonly ConnectionFactory _connectionFactory;

        /// <summary>
        /// Inicializa uma nova instância do consumidor de fila.
        /// </summary>
        public QueueConsumer()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = "localhost", // Altere conforme necessário
                UserName = "guest",
                Password = "guest"
            };
        }

        /// <summary>
        /// Inicia o consumo de mensagens da fila especificada.
        /// </summary>
        /// <param name="queueName">Nome da fila.</param>
        public void Consume(string queueName)
        {
            var connection = _connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Recebido: {message}");
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine($"[x] Consumindo mensagens da fila {queueName}...");
        }
    }
}
