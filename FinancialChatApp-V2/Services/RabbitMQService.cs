using FinancialChatApp_V2.Data;
using FinancialChatApp_V2.Models;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Security;
using System.Text;

namespace FinancialChatApp_V2.Services
{
    public class RabbitMQService : IAsyncDisposable
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IChannel _channel;

        public RabbitMQService()
        {
            _factory = new ConnectionFactory
            {
                HostName = "chimpanzee.rmq.cloudamqp.com", // Replace with your RabbitMQ server address
                Port = 5671,            // Default RabbitMQ port
                UserName = "qptqrako",     // Default RabbitMQ username
                Password = "gzOkaYXAOF7o5FruK3guMcpwWjeZ1H--",
                VirtualHost = "qptqrako",
                Ssl = new SslOption
                {
                    Enabled = true, // Enable SSL/TLS for secure communication
                    ServerName = "chimpanzee.rmq.cloudamqp.com", // Match the host name
                    AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNameMismatch // Allow certificate mismatch
                },
                RequestedHeartbeat = TimeSpan.FromSeconds(30)
            };
        }

        public async Task InitializeAsync()
        {
            _connection = await _factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(
                  queue: "ChatQueue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );
        }

        public async Task<Task> SendMessageAsync(ChatMessage message)
        {

            var body = Encoding.UTF8.GetBytes(message.Content);
            await _channel.BasicPublishAsync(
                exchange: "",
                routingKey: "ChatQueue",
                mandatory: false,  // Use mandatory flag
                body: body
                );

            return Task.CompletedTask;
        }

        public async Task<Task> ReceiveMessageAsync(Func<string, Task> onMessageReceived)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await onMessageReceived(message);
            };

            await _channel.BasicConsumeAsync(
                  queue: "ChatQueue",
                  autoAck: true,
                  consumer: consumer
             );

            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync() 
        {
            _channel?.Dispose();
            _connection?.Dispose();
            return ValueTask.CompletedTask;
            
        }
    }
}