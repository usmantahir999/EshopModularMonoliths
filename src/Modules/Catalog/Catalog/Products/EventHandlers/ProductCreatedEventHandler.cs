
namespace Catalog.Products.EventHandlers
{
    public class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger) : INotificationHandler<ProductCreatedEvent>
    {
        public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            // Handle the product created event here
            // For example, you could log the event or send a notification
            logger.LogInformation("Domain event handled: {DomainEvent}", notification.GetType().Name);
             await Task.CompletedTask;
        }
    }
   
}
