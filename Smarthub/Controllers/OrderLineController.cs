using Microsoft.AspNetCore.Mvc;
using Smarthub.API.Models;
using Smarthub.Service;
using Smarthub.ViewModels;

namespace Smarthub.Controllers
{
    public class OrderLineController : Controller
    {
        private readonly OrderServiceRepository _orderService;

        public OrderLineController(OrderServiceRepository orderService )
        {
            _orderService = orderService;

           
        }
        [HttpGet]
        public async Task<IActionResult> GetOrderLines()
        {
            var orderLines = await _orderService.GetOrderLinesAsync();

            return View(orderLines);
        }

        public async Task<IActionResult> OrderLineDetails(Guid OrderLineId)
        {
            try
            {
                var orderLine = await _orderService.GetOrderLineAsyncById(OrderLineId);


                if (orderLine == null)
                {
                    return NotFound();
                }

                return View(orderLine);
            }
            catch (Exception)
            {

                throw new Exception(" An error occurred while retrieving the order line.");
            }

        }

        [HttpGet]
        public async Task<IActionResult> AddOrderLine(Guid OrderId)
        {
            try
            {
                var orderLine = _orderService.GetOrderAsyncById(OrderId);

                if(orderLine == null)
                {
                    return NotFound();
                }

            }catch(Exception)
            {
                throw new Exception("An error occurred while trying to ");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddOrderLine(OrderLineDTO orderLineDTO, Guid OrderId)
        {
            if (!ModelState.IsValid)
            {
                return View(orderLineDTO);
            }

            try
            {
                var createdOrderLine = await _orderService.AddOrderLineAsync(orderLineDTO);

                if (createdOrderLine != null)
                {
                    ViewBag.OrderId = OrderId;
                    TempData["success"] = "Order Line successfully saved!";
                    return View(); 
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create order line.");
                }
            }
            catch (Exception ex)
            {
                
                

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the order line.");
            }
        }

        public async Task<IActionResult> EditOrderLine(Guid OrderLineId)
        {
            try
            {
                var orderLine = await _orderService.GetOrderLineAsyncById(OrderLineId);

                if (orderLine == null)
                {
                    return NotFound();
                }

                return View(orderLine);
            }
            catch(Exception)
            {
                throw new Exception("AN error occurred while loading order line");
            }
          

            
        }


        [HttpPost]
        public async Task<IActionResult> EditOrderLine(Guid OrderLineId, OrderLineDTO orderLineDTO)
        {
            if (OrderLineId != orderLineDTO.OrderLineId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(orderLineDTO);
            }

            try
            {
                await _orderService.UpdateOrderLineAsync(orderLineDTO);

                TempData["success"] = "Post successfully updated!";

                return View(orderLineDTO);
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while updating the order line.");
            }
        }



        public async Task<IActionResult> DeleteOrderLine(Guid OrderLineId, OrderLineDTO orderLineDTO)
        {
            try
            {
                var orderLine = await _orderService.GetOrderLineAsyncById(OrderLineId);

                if (orderLine == null)
                {
                    return NotFound();
                }

                await _orderService.DeleteOrderLineAsync(OrderLineId);

                TempData["success"] = "order line successfully deleted!";

                return View(orderLineDTO);

            }
            catch (Exception)
            {
                throw new Exception("An error occurred while deleting the order line.");
            }
        }

    }
}
