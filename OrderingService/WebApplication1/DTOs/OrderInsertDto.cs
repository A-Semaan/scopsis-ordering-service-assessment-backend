using OrderingServiceData.Entities;

namespace OrderingServiceWeb.DTOs
{
    public class OrderInsertDto
    {
        public required List<long> ItemIDs { get; set; }
    }
}
