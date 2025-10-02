using OrderingServiceData.Entities;

namespace OrderingServiceWeb.DTOs
{
    public class OrderInsertDto
    {
        public Dictionary<long,int> ItemIDs { get; set; }
    }
}
