using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

        public static async Task<RabbitMqEmbeddingJobQueue> CreateAsync(string host, string user, string password)
        {

            var factory = new ConnectionFactory
            {
                HostName = host,
                UserName = user,
                Password = password,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                RequestedConnectionTimeout = TimeSpan.FromSeconds(30)
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

        public Task SubscribeAsync(
            Func<EmbeddingJob, CancellationToken, Task> handle,
            bool autoAck = false,
            CancellationToken cancellationToken = default
        )
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var job = JsonSerializer
                          .Deserialize<EmbeddingJob>(ea.Body.ToArray())
                          ?? throw new InvalidOperationException("Invalid payload");

                await handle(job, cancellationToken)
                      .ConfigureAwait(false);

                if (!autoAck)
                    await _channel.BasicAckAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,
                        cancellationToken
                    ).ConfigureAwait(false);
            };

            // starts consuming
            return _channel.BasicConsumeAsync(
                queue: QueueName,
                autoAck: autoAck,
                consumer: consumer,
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
