using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrderSpecyfication : BaseSpecification<Order>
    {
        public OrderSpecyfication(string email) : base(x => x.BuyerEamil == email)
        {
            AddInclude(x => x.OrderItems);
            AddInclude(x => x.DeliveryMethod);
            AddOrderByDescending(x => x.OrderDate);
        }

        public OrderSpecyfication(string email, int id) 
            : base(x =>x.BuyerEamil == email && x.Id == id)
        {
            AddInclude("OrderItems");
            AddInclude("DeliveryMethod");
        }
    }
}
