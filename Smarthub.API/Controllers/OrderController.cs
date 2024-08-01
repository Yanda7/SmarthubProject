#region using directives
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smarthub.API.Data;
using Smarthub.API.Models;
using Smarthub.API.Repository;
using Smarthub.API.Services;
using Smarthub.API.Utility;
using Smarthub.API.ViewModels;
using static Smarthub.API.Services.IUnitOfWork;
#endregion

namespace Smarthub.API.Controllers
{
    #region Order Controller

    /// <summary>
    /// Controller for managing orders
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        #region Private Fields

        /// <summary>
        /// The unit of work for orders, providing a way to manage database transactions
        /// </summary>
        private readonly IUnitOfWork<Order> _orderContext;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the OrderController class
        /// </summary>
        /// <param name="orderContext">The unit of work for orders</param>
        public OrderController(IUnitOfWork<Order> orderContext)
        {
            /// <summary>
            /// Checks if the order context is null and throws an ArgumentNullException if it is
            /// </summary>
            _orderContext = orderContext ?? throw new ArgumentNullException(nameof(orderContext));

        }
        #endregion

        #region GET Actions

        /// <summary>
        /// Gets a list of all orders
        /// </summary>
        /// <returns>A list of orders</returns>

        [HttpGet]
        public async Task<ActionResult> GetOrders()
        {
            var orders = await _orderContext.OnLoadItemsAsync();

            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }
        #endregion


        #region Get Order by Id

        /// <summary>
        /// Gets an order by its Id
        /// </summary>
        /// <param name="OrderId">The Id of the order to retrieve</param>
        /// <returns>The order with the specified Id, or NotFound if not found</returns>

        [HttpGet("{OrderId}")]
        public async Task<ActionResult> GetOrderById(Guid OrderId)
        {

            var order = await _orderContext.OnLoadItemAsync(OrderId);


            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
        #endregion

        [HttpPost]
        public async Task<ActionResult> AddOrder(OrderDTO orderDTO)
        {
            try
            {
                if (orderDTO == null)
                {
                    return NotFound();
                }


                Order order = new()
                {
                    OrderId = Helper.GenerateGuid(),

                    OrderCreatedDate = DateTime.Now,

                    OrderDate = DateTime.Now,

                    CustomerName = orderDTO.CustomerName,

                    OrderNumber = orderDTO.OrderNumber,

                    OrderStatus = orderDTO.OrderStatus,

                    OrderType = orderDTO.OrderType,
                    
                };

                if (ModelState.IsValid)
                {
                    await _orderContext.OnItemCreationAsync(order);

                    await _orderContext.ItemSaveAsync();

                    return Ok();
                }

                return StatusCode(500, "error occurred");
            }
            catch (Exception)
            {
                
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPut("{OrderId}")]
        public async Task<ActionResult> ModifyOrder(OrderDTO orderDTO)
        {
           
            if(orderDTO == null)
            {
                return NotFound();
            }

            Order orders = new()
            {
                OrderId = orderDTO.OrderId,
                
                OrderCreatedDate = orderDTO.OrderCreatedDate,

                OrderDate = orderDTO.OrderDate,

                CustomerName = orderDTO.CustomerName,

                OrderNumber = orderDTO.OrderNumber,

                OrderStatus = orderDTO.OrderStatus,

                OrderType = orderDTO.OrderType,

            };

            if (ModelState.IsValid)
            {
                await _orderContext.OnModifyItemAsync(orders);

                await _orderContext.ItemSaveAsync();

                return Ok(orders);
            }

            return StatusCode(500);
        }

        [HttpDelete("{OrderId}")]
        public async Task<ActionResult> RemoveOrder(Guid OrderId)
        {

           await _orderContext.OnRemoveItemAsync(OrderId);

            await _orderContext.ItemSaveAsync();

            return Ok();
        }




    }
    #endregion



}
