using OrderingServiceEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceEngine.Managers.Abstractions
{
    public interface IItemManager
    {
        public long InsertItem(long customerID, ItemModel item);
        public bool UpdateItem(long customerID, long itemID, string? name, string? description, double? price);
        public bool DeleteItem(long customerID, long itemID);
        public ItemModel? GetItem(long customerID, long itemID);
        public List<ItemModel> GetItems(List<long> itemIDs);
        public List<ItemModel> GetAllItems();
    }
}
