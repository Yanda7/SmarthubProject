#region using directives
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smarthub.API.Models;
using Smarthub.API.Services;
using Smarthub.API.Utility;
using Smarthub.API.ViewModels;
using static Smarthub.API.Services.IUnitOfWork;
#endregion

namespace Smarthub.API.Controllers
{
    #region Order Line Controller

    /// <summary>
    /// Controller for managing order lines
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderLineController : ControllerBase
    {
        #region Private Fields

        /// <summary>
        /// The unit of work for order lines, providing a way to manage database transactions
        /// </summary>
        private readonly IUnitOfWork<OrderLine> _context;

        #endregion


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the OrderLineController class
        /// </summary>
        /// <param name="context">The unit of work for order lines</param>
        public OrderLineController(IUnitOfWork<OrderLine> context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion


        #region GET Actions

        /// <summary>
        /// Gets a list of all order lines
        /// </summary>
        /// <returns>A list of order lines</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderLine>>> GetOrderLines()
        {
            var order = await _context.OnLoadItemsAsync();

            if (order == null)
            {
                return NotFound();
            }

            return Ok();
        }

        #endregion


        #region Get Order Line by Id

        /// <summary>
        /// Gets an order line by its Id
        /// </summary>
        /// <param name="OrderLineId">The Id of the order line to retrieve</param>
        /// <returns>The order line with the specified Id, or NotFound if not found</returns>

        [HttpGet("{OrderLineId}")]
        public async Task<ActionResult> GetOrderLine(Guid OrderLineId)
        {
            var orderLine = await _context.OnLoadItemAsync(OrderLineId);

            if (orderLine == null)
            {
                return NotFound();
            }

            return Ok(orderLine);
        }
        #endregion

        #region Create Order Line

        /// <summary>
        /// Creates a new order line
        /// </summary>
        /// <param name="OrderId">The Id of the order to which the order line belongs</param>
        /// <param name="orderLineDTO">The order line data to create</param>
        /// <returns>The created order line, or an error status code if creation fails</returns>

        [HttpPost("api/OrderLine/{OrderId}")]
        public async Task<ActionResult> CreateOrderLine(Guid OrderId, OrderLineDTO orderLineDTO)
        {

            OrderLine orderLine = new()
            {
                OrderLineId = Helper.GenerateGuid(),

                LineNumber = orderLineDTO.LineNumber,

                ProductCode = orderLineDTO.ProductCode,

                ProductType = orderLineDTO.ProductType,

                ProductCostPrice = orderLineDTO.ProductCostPrice,

                ProductSalesPrice = orderLineDTO.ProductSalesPrice,

                Quantity = orderLineDTO.Quantity,

                OrderId = OrderId
            };

            if (ModelState.IsValid)
            {
                await _context.OnItemCreationAsync(orderLine);

                await _context.ItemSaveAsync();

                return Ok(orderLine);
            }

            return StatusCode(500);
        }

        #endregion


        #region Update Order Line

        /// <summary>
        /// Updates an existing order line
        /// </summary>
        /// <param name="OrderId">The Id of the order to which the order line belongs</param>
        /// <param name="OrderLineId">The Id of the order line to update</param>
        /// <param name="orderLineDTO">The updated order line data</param>
        /// <returns>Ok if the update is successful, or an error status code if the update fails</returns>
        /// 
        [HttpPut("{OrderLineId}")]
        public async Task<ActionResult> UpdateOrderLine(Guid OrderId, Guid OrderLineId, OrderLineDTO orderLineDTO)
        {
            if (OrderLineId != orderLineDTO.OrderLineId)
            {
                return BadRequest();
            }

            OrderLine orderLine = new ()
            {
                OrderLineId = orderLineDTO.OrderLineId,

                LineNumber = orderLineDTO.LineNumber,

                ProductCode = orderLineDTO.ProductCode,

                ProductType = orderLineDTO.ProductType,

                ProductCostPrice = orderLineDTO.ProductCostPrice,

                ProductSalesPrice = orderLineDTO.ProductSalesPrice,

                Quantity = orderLineDTO.Quantity,

                OrderId = OrderId
            };

            await _context.OnModifyItemAsync(orderLine);

            await _context.ItemSaveAsync();

            return Ok();
        }
        #endregion


        #region Order Line Deletion

        /// <summary>
        /// Deletes an order line specified by the OrderLineId.
        /// </summary>
        /// <param name="OrderLineId">The unique identifier of the order line to delete.</param>
        /// <returns>An action result indicating the outcome of the operation.</returns>
        /// 
        [HttpDelete("{OrderLineId}")]
        public async Task<ActionResult> DeleteOrderLine( Guid OrderLineId)
        {
            await _context.OnRemoveItemAsync(OrderLineId);

            await _context.ItemSaveAsync();

            return Ok();
        }
        #endregion


    }

    #endregion
}
