using Core.Application.Dtos;

namespace Core.Application.Commands.ShippingCommands
{
    public class UpdateShippingAddressCommand
    {
        public int OrderId { get; set; }
        public ShippingAddressDto ShippingAddress { get; set; }
    }
}
