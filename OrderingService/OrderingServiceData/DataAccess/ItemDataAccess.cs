using OrderingServiceData.DataAccess.Abstractions;
using OrderingServiceData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceData.DataAccess
{
    public class ItemDataAccess : IItemDataAccess
    {
        private readonly OrderingServiceDbContext _dbContext;

        public ItemDataAccess(OrderingServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool DeleteItem(long itemID)
        {
            Item? item = _dbContext.Items.Find(itemID);

            if (item == null)
            {
                throw new Exception("Item not found");
            }

            item.Deleted = true;
            item.DeletedOn = DateTime.UtcNow;

            return _dbContext.SaveChanges() > 0;
        }

        public Item? GetItem(long itemID)
        {
            Item? item = _dbContext.Items.Find(itemID);

            return item;
        }

        public List<Item> GetItems(List<long> itemIDs)
        {
            return _dbContext.Items.Where(item => itemIDs.Contains(item.ID)).ToList();
        }

        public List<Item> GetAllItems()
        {
            return _dbContext.Items.ToList();
        }

        public long InsertItem(Item item)
        {
            _dbContext.Items.Add(item);

            if (_dbContext.SaveChanges() == 0)
            {
                throw new Exception("Failed to insert item");
            }

            return item.ID;
        }

        public bool UpdateItem(long itemID, string? name, string? description, double? price)
        {
            Item? temp = _dbContext.Items.Find(itemID);

            if (temp == null)
            {
                throw new Exception("Item not found");
            }

            if (!string.IsNullOrWhiteSpace(name))
                temp.Name = name;

            if (!string.IsNullOrWhiteSpace(description))
                temp.Description = description;

            if (price.HasValue)
                temp.Price = price.Value;

            return _dbContext.SaveChanges() > 0;
        }
    }
}
