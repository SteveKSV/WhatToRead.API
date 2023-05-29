namespace Core.Application.Interfaces
{
    public interface IShippingService
    {
        Task<ShippingResult> ShipOrderAsync(int orderId);
        Task<ShippingResult> TrackShipmentAsync(int orderId);
        Task<bool> IsShipmentDeliveredAsync(int orderId);
    }
    public class ShippingResult
    {
        public bool IsSuccessful { get; set; }
        public string TrackingNumber { get; set; }
        public string ErrorMessage { get; set; }
    }
}
