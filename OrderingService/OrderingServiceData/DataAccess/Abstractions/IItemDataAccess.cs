using OrderingServiceData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceData.DataAccess.Abstractions
{
    public interface IItemDataAccess
    {
        public long InsertItem(Item item);

        public bool UpdateItem(long itemID, string? name, string? description, double? price);
        
        public Item? GetItem(long itemID);

        public bool DeleteItem(long itemID);

        public List<Item> GetItems(List<long> itemIDs);

        public List<Item> GetAllItems();
    }
}
