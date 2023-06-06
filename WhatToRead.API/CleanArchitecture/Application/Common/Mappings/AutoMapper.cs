using Application.Features.Orders.Queries.GetAllOrders;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Order, GetAllOrdersDto>()
                .ForMember(dest => dest.StatusString, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
