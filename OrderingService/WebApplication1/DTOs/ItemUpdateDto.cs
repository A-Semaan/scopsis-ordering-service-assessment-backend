namespace OrderingServiceWeb.DTOs
{
    public class ItemUpdateDto
    {
        public required long ItemID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
    }
}
