using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Orders.Queries.GetByIdOrder
{
    public class GetByIdOrderQuery : IRequest<Order>
    {
        public int Id { get; set; }
    }

    internal class GetByIdOrderQueryHandler : IRequestHandler<GetByIdOrderQuery, Order>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdOrderQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> Handle(GetByIdOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(request.Id);
            return order;
        }
    }
}
