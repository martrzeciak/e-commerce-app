using API.DTOs;
using Core.Entities.OrderAggregate;

namespace API.Extensions
{
    public static class PaymentSummaryMappingExtenions
    {
        public static PaymentSummaryDto ToDto(this PaymentSummary paymentSummary)
        {
            if (paymentSummary == null)
                throw new ArgumentNullException(nameof(paymentSummary));

            return new PaymentSummaryDto
            {
                Last4 = paymentSummary.Last4,
                Brand = paymentSummary.Brand,
                ExpMonth = paymentSummary.ExpMonth,
                ExpYear = paymentSummary.ExpYear
            };
        }

        public static PaymentSummary ToEntity(this PaymentSummaryDto paymentSummaryDto)
        {
            if (paymentSummaryDto == null) 
                throw new ArgumentNullException(nameof(paymentSummaryDto));

            return new PaymentSummary
            {
                Last4 = paymentSummaryDto.Last4,
                Brand = paymentSummaryDto.Brand,
                ExpMonth = paymentSummaryDto.ExpMonth,
                ExpYear = paymentSummaryDto.ExpYear
            };
        }
    }
}
