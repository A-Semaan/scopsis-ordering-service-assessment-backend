using AutoMapper;
using OrderingServiceData.DataAccess;
using OrderingServiceData.DataAccess.Abstractions;
using OrderingServiceData.Entities;
using OrderingServiceEngine.Managers.Abstractions;
using OrderingServiceEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceEngine.Managers
{
    public class ItemManager : IItemManager
    {

        private readonly IItemDataAccess _itemDataAccess;

        private readonly IApplicationLogManager _applicationLogManager;
        private readonly IMapper _mapper;

        public ItemManager(IItemDataAccess itemDataAccess, IApplicationLogManager applicationLogManager, IMapper mapper)
        {
            _itemDataAccess = itemDataAccess;
            _applicationLogManager = applicationLogManager;
            _mapper = mapper;
        }

        public long InsertItem(long customerID, ItemModel item)
        {
            long itemID = _itemDataAccess.InsertItem(_mapper.Map<Item>(item));

            if (itemID > 0)
            {
                _applicationLogManager.InsertLog(APPLICATION_LOG_EVENT.ITEM_CREATED, itemID.ToString(), customerID);
                return itemID;
            }

            return -1;
        }

        public bool DeleteItem(long customerID, long itemID)
        {
            if (_itemDataAccess.DeleteItem(itemID))
            {
                _applicationLogManager.InsertLog(APPLICATION_LOG_EVENT.ITEM_DELETED, itemID.ToString(), customerID);
                return true;
            }

            return false;
        }

        public List<ItemModel> GetItems(List<long> itemIDs)
        {
            return _mapper.Map<List<ItemModel>>(_itemDataAccess.GetItems(itemIDs));
        }

        public List<ItemModel> GetAllItems()
        {
            return _mapper.Map<List<ItemModel>>(_itemDataAccess.GetAllItems());
        }

        public ItemModel? GetItem(long customerID, long itemID)
        {
            ItemModel? temp = _mapper.Map<ItemModel?>(_itemDataAccess.GetItem(itemID));

            if (temp != null)
            {
                _applicationLogManager.InsertLog(APPLICATION_LOG_EVENT.ITEM_VISITED, itemID.ToString(), customerID);
                return temp;
            }

            return null;
        }

        public bool UpdateItem(long customerID, long itemID, string? name, string? description, double? price)
        {
            if (price.HasValue && price < 0)
            {
                throw new Exception("Price cannot be negative");
            }

            if (_itemDataAccess.UpdateItem(itemID, name, description, price))
            {
                _applicationLogManager.InsertLog(APPLICATION_LOG_EVENT.ITEM_UPDATED, itemID.ToString(), customerID);
                return true;
            }

            return false;
        }
    }
}
