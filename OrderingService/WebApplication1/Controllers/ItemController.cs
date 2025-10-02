using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderingServiceEngine.Managers;
using OrderingServiceEngine.Managers.Abstractions;
using OrderingServiceEngine.Models;
using OrderingServiceWeb.DTOs;
using OrderingServiceWeb.Helpers.Abstractions;

namespace OrderingServiceWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemManager _itemManager;
        private readonly ISecurityHelper _securityHelper;

        public ItemController(IItemManager itemManager, ISecurityHelper securityHelper)
        {
            _itemManager = itemManager;
            _securityHelper = securityHelper;
        }

        [HttpGet("GetItem")]
        public ActionResult<ItemModel> GetItem(long itemID)
        {
            // Validate input
            if (itemID <= 0)
            {
                return BadRequest("Invalid item ID");
            }

            long customerID = _securityHelper.GetCustomerID();

            // get item
            ItemModel? itemModel = _itemManager.GetItem(customerID, itemID);
            if (itemModel == null)
            {
                return NotFound("Item not found");
            }
            return itemModel;
        }

        [HttpGet("GetAllItems")]
        public ActionResult<List<ItemModel>> GetAllItems()
        {
            // get all items
            List<ItemModel> itemModels = _itemManager.GetAllItems();
            
            return itemModels;
        }

        [HttpGet("GetItems")]
        public ActionResult<List<ItemModel>> GetItems(List<long> itemIDs)
        {
            // get specific items
            List<ItemModel> itemModels = _itemManager.GetItems(itemIDs);
            
            return itemModels;
        }

        [HttpPost("InsertItem")]
        public ActionResult<long> InsertItem(ItemInsertDto itemDTO)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(itemDTO.Name) || itemDTO.Price <= 0)
            {
                return BadRequest("Invalid item data");
            }

            long customerID = _securityHelper.GetCustomerID();
            // Create item
            ItemModel itemModel = new ItemModel
            {
                Name = itemDTO.Name,
                Description = itemDTO.Description,
                Price = itemDTO.Price
            };

            return _itemManager.InsertItem(customerID, itemModel);
        }

        [HttpPut("UpdateItem")]
        public ActionResult<bool> UpdateItem(ItemUpdateDto itemDTO)
        {
            // Validate input
            if(itemDTO.ItemID <= 0)
            {
                return BadRequest("Invalid item ID");
            }

            if (string.IsNullOrWhiteSpace(itemDTO.Name) && string.IsNullOrWhiteSpace(itemDTO.Description) && itemDTO.Price == null)
            {
                return BadRequest("At least one of the Name, Description, or price is required");
            }

            long customerID = _securityHelper.GetCustomerID();

            // Update item
            return _itemManager.UpdateItem(customerID, itemDTO.ItemID, itemDTO.Name, itemDTO.Description, itemDTO.Price);
        }

        [HttpDelete("DeleteItem")]
        public ActionResult<bool> DeleteItem(long itemID)
        {
            // Validate input
            if (itemID <= 0)
            {
                return BadRequest("Invalid item ID");
            }

            long customerID = _securityHelper.GetCustomerID();

            // Delete item
            bool success = _itemManager.DeleteItem(customerID, itemID);
            if (!success)
            {
                return StatusCode(500, "Failed to delete item");
            }
            return Ok();
        }
    }
}
