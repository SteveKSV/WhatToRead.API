using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public DateTime NewOrderDate { get; set; }
        public OrderStatus NewStatus { get; set; }
        public decimal NewTotalAmount { get; set; }
    }

    internal class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(request.Id);

            if (order != null)
            {
                // Perform the necessary updates on the order entity
                order.OrderDate = request.NewOrderDate;
                order.Status = request.NewStatus;
                order.TotalAmount = request.NewTotalAmount;

                await _unitOfWork.Repository<Order>().UpdateAsync(order);
                await _unitOfWork.Save(cancellationToken);

                return true; // Return true to indicate successful update
            }

            return false; // Return false if the order with the specified Id was not found
        }
    }
}
