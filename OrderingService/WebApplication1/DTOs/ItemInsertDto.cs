namespace OrderingServiceWeb.DTOs
{
    public class ItemInsertDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
    }
}
