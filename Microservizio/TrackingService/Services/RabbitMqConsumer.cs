using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using SharedModels.Models;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TrackingService.Data;

namespace TrackingService.Services
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string _queueName = "trackingQueue";
        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMqConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672"),
                // AutomaticRecoveryEnabled = true,
                // NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
            };

            try
            {
                _connection = await factory.CreateConnectionAsync("TrackingServiceConnection");
                _channel = await _connection.CreateChannelAsync();

                // Dichiarazione della coda, se non già esistente
                await _channel.QueueDeclareAsync(
                    queue: _queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (sender, ea) =>
                {
                    await ProcessReceivedMessageAsync(ea);
                };

                // Avvio dell'ascolto sulla coda
                await _channel.BasicConsumeAsync(
                    queue: _queueName,
                    autoAck: false,
                    consumer
                );
            }
            catch (BrokerUnreachableException ex)
            {
                // Log di errore connessione
                Console.WriteLine($"❌ Errore connessione RabbitMQ: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Errore generico: {ex.Message}");
            }
        }

        private async Task ProcessReceivedMessageAsync(BasicDeliverEventArgs ea)
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Deserializza direttamente in EventTracking 
                // (che ora contiene MouseX, MouseY, ElementLeft, ecc.)
                var trackingEvent = JsonSerializer.Deserialize<EventTracking>(message);
                Console.WriteLine($"📥 Messaggio ricevuto: {message}");

                if (trackingEvent != null)
                {
                    // Crea uno scope per ottenere il DbContext
                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    // Verifica se la sessione esiste
                    var session = await dbContext.Sessions
                        .FirstOrDefaultAsync(s => s.Id == trackingEvent.SessionId);

                    if (session == null)
                    {
                        // Se non esiste, la creiamo (default placeholders)
                        session = new SessionTracking
                        {
                            Id = trackingEvent.SessionId,
                            UserId = trackingEvent.UserId,
                            StartTimeUtc = DateTime.UtcNow,
                            IpAddress = "0.0.0.0",
                            BrowserInfo = "Unknown",
                            DeviceType = "Unknown",
                            OperatingSystem = "Unknown",
                            Locale = "it-IT",
                            PageViews = 0,
                            OptOut = false
                        };

                        await dbContext.Sessions.AddAsync(session);
                        Console.WriteLine($"✅ Nuova sessione creata: {trackingEvent.SessionId}");
                    }

                    // Eventuali logiche specifiche a seconda del tipo di evento
                    switch (trackingEvent.EventName)
                    {
                        case "PageView":
                            // Incrementa il contatore pageViews 
                            session.PageViews += 1;
                            Console.WriteLine("📝 Evento PageView: incrementato PageViews");
                            break;

                        case "Click":
                            // Esempio di log delle nuove colonne 
                            Console.WriteLine($"🖱 Evento Click: " +
                                $"Mouse=({trackingEvent.MouseX}, {trackingEvent.MouseY}), " +
                                $"ElemRect=({trackingEvent.ElementLeft}, {trackingEvent.ElementTop}, " +
                                $"{trackingEvent.ElementWidth}, {trackingEvent.ElementHeight})");
                            break;

                        case "SessionReplayChunk":
                            // Esempio: potresti avere i dati di replay in ReplayChunkData
                            Console.WriteLine($"🎬 Replay chunk ricevuto, type={trackingEvent.ReplayChunkType}, " +
                                $"chunk size={trackingEvent.ReplayChunkData?.Length}");
                            break;

                        case "FunnelStep":
                            // Esempio: log dell'info funnel
                            Console.WriteLine($"🔄 Funnel step: {trackingEvent.FunnelData}");
                            break;

                        default:
                            Console.WriteLine($"Evento generico: {trackingEvent.EventName}");
                            break;
                    }

                    await dbContext.Events.AddAsync(trackingEvent);
                    await dbContext.SaveChangesAsync();

                    switch (trackingEvent.EventName)
                    {
                        case "PageView":
                            LiveAggregator.IncrementCounter("PageView");
                            break;
                        case "Click":
                            LiveAggregator.IncrementCounter("Click");
                            break;
                        case "FunnelStep":
                            LiveAggregator.IncrementCounter($"Funnel_{trackingEvent.FunnelStep}");
                            break;
                            // etc.
                    }

                    Console.WriteLine("✅ Evento salvato nel database!");
                    // Se TimestampUtc non è impostato, lo impostiamo ora
                    if (trackingEvent.TimestampUtc == default)
                    {
                        trackingEvent.TimestampUtc = DateTime.UtcNow;
                    }

                    // Salvataggio dell'evento
                    await dbContext.Events.AddAsync(trackingEvent);
                    await dbContext.SaveChangesAsync();

                    Console.WriteLine("✅ Evento salvato nel database!");
                }

                // Conferma del messaggio a RabbitMQ
                await _channel!.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (JsonException jsonEx)
            {
                // Messaggio non deserializzabile
                Console.WriteLine($"❌ Errore deserializzazione JSON: {jsonEx.Message}");
                // Decidi se ri-mettere in coda (true) o scartare (false)
                await _channel!.BasicNackAsync(ea.DeliveryTag, false, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Errore elaborazione messaggio: {ex.Message}");
                await _channel!.BasicNackAsync(ea.DeliveryTag, false, false);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            if (_channel != null)
                await _channel.DisposeAsync();

            if (_connection != null)
                await _connection.DisposeAsync();

            await base.StopAsync(stoppingToken);
        }
    }
}
