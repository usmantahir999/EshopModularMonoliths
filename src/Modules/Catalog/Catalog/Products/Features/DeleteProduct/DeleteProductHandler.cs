﻿
namespace Catalog.Products.Features.DeleteProduct
{
    public record DeleteProductCommand(int ProductId) : ICommand<DeleteProductResult>;

    public record DeleteProductResult(bool IsSuccess);

    internal class DeleteProductHandler(CatalogDbContext dbContext) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await dbContext.Products.FindAsync([command.ProductId],cancellationToken);
            if(product is null)
            {
                throw new Exception($"Product with ID {command.ProductId} not found.");
            }
            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new DeleteProductResult(true);
        }
    }
}
