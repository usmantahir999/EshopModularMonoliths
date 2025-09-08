using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Basket.Data.Processors
{
    public class OutboxProcessor(IServiceProvider serviceProvider, IBus bus, ILogger<OutboxProcessor> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                    var outboxMessages = await dbContext.OutboxMessages.Where(x => x.ProcessedOn == null).ToListAsync(stoppingToken);

                    foreach (var message in outboxMessages)
                    {
                        var eventType = Type.GetType(message.Type);
                        if(eventType == null)
                        {
                            logger.LogError("Could not load type {Type} from outbox message {MessageId}", message.Type, message.Id);
                            continue;
                        }

                        var eventMessage = JsonSerializer.Deserialize(message.Data, eventType);
                        if(eventMessage == null)
                        {
                            logger.LogError("Could not deserialize message data to type {Type} from outbox message {MessageId}", message.Type, message.Id);
                            continue;
                        }

                        await bus.Publish(eventMessage, stoppingToken);
                        message.ProcessedOn = DateTime.UtcNow;
                        logger.LogInformation("Published outbox message {MessageId} of type {Type}", message.Id, message.Type);
                    }
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing outbox messages");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
