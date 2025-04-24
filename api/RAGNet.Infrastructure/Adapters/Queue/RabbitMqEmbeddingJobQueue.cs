using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RAGNET.Domain.Entities.Jobs;
using RAGNET.Domain.Services.Queue;

namespace RAGNET.Infrastructure.Adapters.Queue
{
    public class RabbitMqEmbeddingJobQueue : IEmbeddingJobQueue, IAsyncDisposable
    {
        private const string QueueName = "embedding_jobs";
        private readonly IConnection _connection;
        private readonly IChannel _channel;

        private RabbitMqEmbeddingJobQueue(IConnection connection, IChannel channel)
        {
            _connection = connection;
            _channel = channel;
        }


        public static async Task<RabbitMqEmbeddingJobQueue> CreateAsync(IConfiguration config)
        {
            var factory = new ConnectionFactory
            {
                HostName = config["RabbitMQ:Host"]
                    ?? throw new Exception("RabbitMQ Host must be set."),
                UserName = config["RabbitMQ:Username"]
                    ?? throw new Exception("RabbitMQ Username must be set."),
                Password = config["RabbitMQ:Password"]
                    ?? throw new Exception("RabbitMQ Password must be set.")
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            return new RabbitMqEmbeddingJobQueue(connection, channel);
        }

        public async Task EnqueueAsync(EmbeddingJob job, CancellationToken cancellationToken = default)
        {
            var body = JsonSerializer.SerializeToUtf8Bytes(job);

            var props = new BasicProperties
            {
                Persistent = true
            };

            await _channel.BasicPublishAsync(
                exchange: "",
                routingKey: QueueName,
                mandatory: false,
                basicProperties: props,
                body: body,
                cancellationToken: cancellationToken
            );
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel.IsOpen)
                await _channel.CloseAsync();
            _channel.Dispose();

            if (_connection.IsOpen)
                await _connection.CloseAsync();
            _connection.Dispose();
        }
    }
}
