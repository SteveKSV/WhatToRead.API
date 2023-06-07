using Application.Features.Orders.Queries.GetAllOrders;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries.GetByIdOrder
{
    public class GetByIdOrderQuery : IRequest<GetByIdOrderDto>
    {
        public string Id { get; set; }
    }

    internal class GetByIdOrderQueryHandler : IRequestHandler<GetByIdOrderQuery, GetByIdOrderDto>
    {
        private readonly IGenericRepository<Order> _repository;
        private readonly IMapper _mapper;

        public GetByIdOrderQueryHandler(IGenericRepository<Order> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetByIdOrderDto> Handle(GetByIdOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(request.Id);

            var dto = _mapper.Map<GetByIdOrderDto>(order);
            return dto;
        }
    }
}
