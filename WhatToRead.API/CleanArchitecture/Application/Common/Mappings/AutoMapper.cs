using Application.Features.Orders.Commands.CreateOrder;
using Application.Features.Orders.Queries.GetAllOrders;
using Application.Features.Orders.Queries.GetByIdOrder;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Order, GetAllOrdersDto>()
                .ForMember(dest => dest.StatusString, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<Order, GetByIdOrderDto>()
                .ForMember(dest => dest.StatusString, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
        }
    }
}
