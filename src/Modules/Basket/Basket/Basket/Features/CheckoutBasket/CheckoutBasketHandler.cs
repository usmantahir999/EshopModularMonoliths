﻿using MassTransit;
using Shared.Messaging.Events;

namespace Basket.Basket.Features.CheckoutBasket
{
    public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout)
    : ICommand<CheckoutBasketResult>;

    public record CheckoutBasketResult(bool IsSuccess);

    public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketCommandValidator()
        {
            RuleFor(x => x.BasketCheckout).NotNull().WithMessage("BasketCheckoutDto can't be null");
            RuleFor(x => x.BasketCheckout.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    internal class CheckoutBasketHandler(IBasketRepository repository, IBus bus) : IRequestHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        public readonly IBus _bus = bus;

        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
        {
            // get existing basket with total price
            // Set totalprice on basketcheckout event message
            // send basket checkout event to rabbitmq using masstransit
            // delete the basket

            var basket = await repository.GetBasket(command.BasketCheckout.UserName, true, cancellationToken);
            if (basket == null)
            {
                throw new BasketNotFoundException(command.BasketCheckout.UserName);
            }
            var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutIntegrationEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;

            await _bus.Publish(eventMessage, cancellationToken);

            await repository.DeleteBasket(command.BasketCheckout.UserName, cancellationToken);

            return new CheckoutBasketResult(true);
        }
    }
}
