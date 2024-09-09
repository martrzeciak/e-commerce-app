using API.DTOs;
using Core.Entities.OrderAggregate;

namespace API.Extensions
{
    public static class ShippingAddressMappingExtensions
    {
        public static ShippingAddressDto ToDto(this ShippingAddress shippingAddress)
        {
            if (shippingAddress == null)
                throw new ArgumentNullException(nameof(shippingAddress));

            return new ShippingAddressDto
            {
                Name = shippingAddress.Name,
                Line1 = shippingAddress.Line1,
                Line2 = shippingAddress.Line2,
                City = shippingAddress.City,
                State = shippingAddress.State,
                PostalCode = shippingAddress.PostalCode,
                Country = shippingAddress.Country
            };
        }

        public static ShippingAddress ToEntity(this ShippingAddressDto shippingAddressDto) 
        {
            if (shippingAddressDto == null) 
                throw new ArgumentNullException(nameof(shippingAddressDto));

            return new ShippingAddress
            {
                Name = shippingAddressDto.Name,
                Line1 = shippingAddressDto.Line1,
                Line2 = shippingAddressDto.Line2,
                City = shippingAddressDto.City,
                State = shippingAddressDto.State,
                PostalCode = shippingAddressDto.PostalCode,
                Country = shippingAddressDto.Country
            };
        }
    }
}
