using TexasTaco.Orders.Application.Shared.DTO;
using TexasTaco.Orders.Domain.Shared;

namespace TexasTaco.Orders.Application.Shared.Mappers
{
    internal static class DeliveryAddressMap
    {
        public static DeliveryAddressDto? Map(DeliveryAddress? deliveryAddress)
        {
            if (deliveryAddress is null)
            {
                return null;
            }

            return new(
                deliveryAddress.ReceiverFullName,
                deliveryAddress.AddressLine,
                deliveryAddress.PostalCode,
                deliveryAddress.City);
        }
    }
}
