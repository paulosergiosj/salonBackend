using MongoDB.Bson;
using Salon.Domain.Models;

namespace Salon.Domain.ServiceOrders.Entities
{
    public class ItemOrder
    {
        public ObjectId ItemId { get; set; }
        public double Value { get; set; }
    }
}
