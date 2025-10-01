using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderingServiceData.Entities
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }

        [Column("name", TypeName = "varchar(200)")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password_hash")]
        public string PasswordHash { get; set; }
    }
}
