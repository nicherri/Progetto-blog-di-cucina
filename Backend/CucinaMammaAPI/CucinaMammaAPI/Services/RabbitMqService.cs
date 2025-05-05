using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CucinaMammaAPI.Services
{
    /// <summary>
    /// Servizio per pubblicare messaggi di tracking su RabbitMQ.
    /// Implementa IDisposable per chiudere la connessione in modo corretto.
    /// </summary>
    public class RabbitMqService : IDisposable
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly string _queueName = "trackingQueue";
        private bool _disposed = false;

        public RabbitMqService()
        {
            // 🔥 MIGLIORIA: potresti gestire la stringa di connessione da config
            _factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672"),
                // AutomaticRecoveryEnabled = true,    // Se vuoi un autorecovery
                // NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
            };

            InitializeRabbitMQ().GetAwaiter().GetResult();
        }

        private async Task InitializeRabbitMQ()
        {
            _connection = await _factory.CreateConnectionAsync("CucinaMammaConnection");
            _channel = await _connection.CreateChannelAsync();

            // 🔥 MIGLIORIA: potresti usare un exchange personalizzato e un binding. 
            // Per semplicità, usiamo la coda diretta "trackingQueue".
            await _channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        /// <summary>
        /// Pubblica un messaggio generico T sulla coda RabbitMQ.
        /// </summary>
        public async Task PublishMessage<T>(T message)
        {
            if (_channel == null)
                throw new InvalidOperationException("RabbitMQ non è stato inizializzato correttamente.");

            try
            {
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                // Impostiamo persistenza
                var properties = new BasicProperties
                {
                    DeliveryMode = DeliveryModes.Persistent
                };

                // 🔥 MIGLIORIA: potresti loggare il messaggio prima di pubblicarlo
                await _channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: _queueName,
                    mandatory: false,
                    basicProperties: properties,
                    body: body
                );
            }
            catch (Exception ex)
            {
                // 🔥 MIGLIORIA: usa un logger (Serilog, NLog, etc.)
                Console.WriteLine($"[RabbitMqService] Errore pubblicazione messaggio: {ex.Message}");
                throw; // Rilancia l'eccezione o gestiscila
            }
        }

        /// <summary>
        /// Chiude la connessione a RabbitMQ.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _channel?.DisposeAsync().AsTask().Wait();
                _connection?.DisposeAsync().AsTask().Wait();
            }
        }
    }
}
