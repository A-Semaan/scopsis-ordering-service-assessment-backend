using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceData.Entities
{
    public class ApplicationLog
    {
        [Key]
        [Column("log_id")]
        public long LogID { get; set; }

        [Column("log_date")]
        public DateTime LogDate { get; set; }

        [Column("event")]
        public APPLICATION_LOG_EVENT Event { get; set; }

        [Column("event_info")]
        public string? EventInfo { get; set; }

        [Column("customer_id")]
        public long? CustomerID { get; set; }

        [ForeignKey(nameof(CustomerID))]
        public Customer? Customer { get; set; }
    }

    public enum APPLICATION_LOG_EVENT
    {
        LOGIN_SUCCESS=1,
        LOGIN_FAILURE=2,
        CUSTOMER_CREATED=3,
        CUSTOMER_UPDATED=4,
        ORDER_CREATED=5,
        ITEM_CREATED=6,
        ITEM_UPDATED=7,
        ITEM_DELETED=8,
        ITEM_VISITED=9,
        LOGS_ACCESSED=10
    }
}
