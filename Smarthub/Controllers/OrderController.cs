using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smarthub.API.Models;
using Smarthub.Service;
using Smarthub.ViewModels;

namespace Smarthub.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderServiceRepository _orderService;

        public OrderController(OrderServiceRepository orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();


            return View(orders);
        }


        public async Task<IActionResult> OrderDetails(Guid OrderId)
        {
            try
            {
                var order = await _orderService.GetOrderAsyncById(OrderId);

                if (order == null)
                {
                    return NotFound();
                }

                return View(order);
            }
            catch (Exception)
            {

                throw new Exception(" An error occurred while retrieving the order.");
            }

        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            return View();
        }

        [HttpPost]      
        
        public async Task<IActionResult> CreateOrder(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(orderDTO);
            }

            try
            {
                var createdOrder = await _orderService.AddOrderAsync(orderDTO);

                if (createdOrder != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create order.");
                }

                TempData["success"] = "Order successfully saved!";


                return View(createdOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the order.");
            }
        }

       
        public async Task<IActionResult> EditOrder(Guid OrderId)
        {
            try
            {
                var order = await _orderService.GetOrderAsyncById(OrderId);

                if (order == null)
                {
                    return NotFound();
                }

                return View(order);
            }
            catch(Exception )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the order.");
            }

        }

        
        [HttpPost]
        public async Task<IActionResult> EditOrder(Guid OrderId, OrderDTO orderDTO)
        {
            if (OrderId != orderDTO.OrderId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(orderDTO);
            }

            try
            {
                await _orderService.UpdateOrderAsync(orderDTO);

                TempData["success"] = "order successfully updated!";

                return View(orderDTO);
            }
            catch (Exception)
            {
                throw new Exception( "An error occurred while updating the order.");
            }
        }

       
        //public async Task<IActionResult> DeleteOrder(Guid OrderId)
        //{

        //    var order = await _orderService.GetOrderAsyncById(OrderId);

        //    if (order == null)
        //    {
        //        return NotFound();
        //    }
        //    return View();
        //}

 
        public async Task<IActionResult> DeleteOrder(Guid OrderId)
        {
            try
            {
                

                await _orderService.DeleteOrderAsync(OrderId);

                TempData["success"] = "order successfully deleted!";

                return View();

            }
            catch (Exception)
            {
                throw new Exception("An error occurred while deleting the order.");
            }
        }

    }
}
