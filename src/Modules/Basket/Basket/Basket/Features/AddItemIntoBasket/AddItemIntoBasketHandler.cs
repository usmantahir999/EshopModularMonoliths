﻿namespace Basket.Basket.Features.AddItemIntoBasket
{
    public record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem)
    : ICommand<AddItemIntoBasketResult>;
    public record AddItemIntoBasketResult(Guid Id);

    public class AddItemIntoBasketCommandValidator : AbstractValidator<AddItemIntoBasketCommand>
    {
        public AddItemIntoBasketCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
            RuleFor(x => x.ShoppingCartItem.ProductId).NotEmpty().WithMessage("ProductId is required");
            RuleFor(x => x.ShoppingCartItem.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }


    class AddItemIntoBasketHandler(IBasketRepository basketRepository): ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
    {
        public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand request, CancellationToken cancellationToken)
        {
            var shoppingCart = await basketRepository.GetBasket(request.UserName,false, cancellationToken);

            shoppingCart.AddItem(
                request.ShoppingCartItem.ProductId,
                request.ShoppingCartItem.Quantity,
                request.ShoppingCartItem.Color,
                request.ShoppingCartItem.Price,
                request.ShoppingCartItem.ProductName
            );
           await basketRepository.SaveChangesAsync(cancellationToken);
           return new AddItemIntoBasketResult(shoppingCart.Id);
        }
    }
}
