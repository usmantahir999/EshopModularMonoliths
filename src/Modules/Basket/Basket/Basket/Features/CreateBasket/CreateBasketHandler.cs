﻿
namespace Basket.Basket.Features.CreateBasket
{
    public record CreateBasketCommand(ShoppingCartDto ShoppingCart)
     : ICommand<CreateBasketResult>;
    public record CreateBasketResult(Guid Id);
    public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
    {
        public CreateBasketCommandValidator()
        {
            RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    internal class CreateBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<CreateBasketCommand, CreateBasketResult>
    {
        public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
        {
            //create Basket entity from command object
            //save to database
            //return result
            var shoppingCart = CreateNewBasket(command.ShoppingCart);
            dbContext.ShoppingCarts.Add(shoppingCart);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new CreateBasketResult(shoppingCart.Id);
        }

        private static ShoppingCart CreateNewBasket(ShoppingCartDto shoppingCartDto)
        {
            var newBasket= ShoppingCart.Create(Guid.NewGuid(), shoppingCartDto.UserName);
            shoppingCartDto.Items.ForEach(item =>
            {
               newBasket.AddItem(item.ProductId, item.Quantity, item.Color, item.Price, item.ProductName);
            });
            return newBasket;
        }
    }
}
