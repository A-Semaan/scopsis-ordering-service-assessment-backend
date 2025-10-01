using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceData.Entities
{
    public class Item
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }

        [Column("name", TypeName = "varchar(200)")]
        public string Name { get; set; }

        [Column("description", TypeName = "varchar(1000)")]
        public string? Description { get; set; }

        [Column("price")]
        public double Price { get; set; }

        [Column("deleted")]
        public bool Deleted { get; set; }

        [Column("deleted_on")]
        public DateTime? DeletedOn { get; set; }
    }
}
