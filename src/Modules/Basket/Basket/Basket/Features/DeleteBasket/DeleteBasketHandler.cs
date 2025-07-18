﻿

namespace Basket.Basket.Features.DeleteBasket
{
    public record DeleteBasketCommand(string UserName)
    : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool IsSuccess);

    internal class DeleteBasketHandler(BasketDbContext dbContext) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await dbContext.ShoppingCarts.SingleOrDefaultAsync(x => x.UserName == command.UserName, cancellationToken);
            if (basket == null)
            {
               throw new BasketNotFoundException(command.UserName);
            }
            dbContext.ShoppingCarts.Remove(basket); 
            await dbContext.SaveChangesAsync(cancellationToken);
            return new DeleteBasketResult(true);
        }
    }
}
