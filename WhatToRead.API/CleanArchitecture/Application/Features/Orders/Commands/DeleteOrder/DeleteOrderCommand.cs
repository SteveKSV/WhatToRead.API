using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Orders.Commands.DeleteOrder
{
    public record class DeleteOrderCommand : IRequest<int>
    {
        public string Id { get; set; }
    }

    internal class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, int>
    {
        private readonly IGenericRepository<Order> _repository;

        public DeleteOrderCommandHandler(IGenericRepository<Order> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await _repository.GetByIdAsync(request.Id);

            if (orderToDelete != null)
            {
                await _repository.DeleteAsync(orderToDelete);

                // No need to call Save for MongoDB
                // Changes are automatically persisted

                // Return 1 to indicate that one order was successfully deleted
                return 1;
            }
            else
            {
                // Return 0 to indicate that no order was deleted (no matching order found)
                return 0;
            }
        }
    }

}
