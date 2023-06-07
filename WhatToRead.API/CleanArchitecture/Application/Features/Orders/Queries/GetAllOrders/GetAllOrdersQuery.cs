using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Orders.Queries.GetAllOrders
{
    public record class GetAllOrdersQuery : IRequest<List<GetAllOrdersDto>>
    {
    }
    internal class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<GetAllOrdersDto>>
    {
        private readonly IGenericRepository<Order> _repository;
        private readonly IMapper _mapper;

        public GetAllOrdersQueryHandler(IGenericRepository<Order> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<GetAllOrdersDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repository.GetAllAsync();

            var dtos = _mapper.Map<List<GetAllOrdersDto>>(orders);
            return dtos;
        }
    }
}
