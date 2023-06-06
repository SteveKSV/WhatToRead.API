using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Orders.Commands.DeleteOrder
{
    public record class DeleteOrderCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    internal class  DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.Repository<Order>().GetAllAsync();
            var orderToDelete = orders.FirstOrDefault(p => p.Id == request.Id);

            if (orderToDelete != null)
            {
                await _unitOfWork.Repository<Order>().DeleteAsync(orderToDelete);
                await _unitOfWork.Save(cancellationToken);

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
