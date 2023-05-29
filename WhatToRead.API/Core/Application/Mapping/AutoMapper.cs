using AutoMapper;
using Core.Application.Dtos;
using Core.Domain.Entities;

namespace Core.Application.Mapping
{
    public class AutoMapper : Profile
    {
        public AutoMapper() {
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<ShippingAddress, ShippingAddressDto>().ReverseMap();
        }
    }
}
