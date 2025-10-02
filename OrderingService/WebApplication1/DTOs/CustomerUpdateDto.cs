namespace OrderingServiceWeb.DTOs
{
    public class CustomerUpdateDto
    {
        public long CustomerID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
