using ClosedXML.Excel;
using CRUD_EF.Models;
using CRUD_EF.Services;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_EF.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index(string searchKeyword,DateTime? orderDate)
        {
            var orders = await _orderService.GetOrdersAsync(searchKeyword,orderDate);
            return View(orders);
        }

        public async Task<IActionResult> Details(long id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        public async Task<IActionResult> Create()
        {
            var customers = await _orderService.GetCustomersAsync();
            ViewBag.Customers = customers ?? new List<COM_CUSTOMER>();
            return View(new SO_ORDER());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SO_ORDER order)
        {
            Console.WriteLine($"OrderNo: {order.OrderNo}, CustomerID: {order.ComCustomerId}");
            Console.WriteLine($"Items Count: {order.Items?.Count ?? 0}");

            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _orderService.GetCustomersAsync();
                ViewBag.Items = order.Items ?? new List<SO_ITEM>();
                return View(order);
            }

            bool result = await _orderService.CreateOrderAsync(order);
            if (result)
            {
                TempData["SuccessMessage"] = "Order created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("","Failed to create order.");
            ViewBag.Customers = await _orderService.GetCustomersAsync();
            ViewBag.Items = order.Items ?? new List<SO_ITEM>();
            return View(order);
        }


        public async Task<IActionResult> Edit(long id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.Customers = await _orderService.GetCustomersAsync();
            ViewBag.Items = order.Items;

            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id,SO_ORDER order)
        {
            if (id != order.SoOrderId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _orderService.GetCustomersAsync();
                return View(order);
            }

            

            order.Items ??= new List<SO_ITEM>(); 
            

            var result = await _orderService.UpdateOrderAsync(order);
            if (!result)
            {
                ModelState.AddModelError(string.Empty,"Failed to update the order.");
                ViewBag.Customers = await _orderService.GetCustomersAsync();
                return View(order);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _orderService.DeleteOrderAsync(id);

            if (result)
            {
                return Json(new { success = true,message = "Order deleted successfully." });
            }
            else
            {
                return Json(new { success = false,message = "Failed to delete order. It may have related items." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel()
        {
            var orders = await _orderService.GetOrdersAsync(null,null); 
            if (orders == null || !orders.Any())
                return BadRequest("No orders available for export.");

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sales Orders");

            worksheet.Cell(1,1).Value = "Sales Order ID";
            worksheet.Cell(1,2).Value = "Order Date";
            worksheet.Cell(1,3).Value = "Customer Name";

            for (int i = 0;i < orders.Count;i++)
            {
                worksheet.Cell(i + 2,1).Value = orders[i].SoOrderId;
                worksheet.Cell(i + 2,2).Value = orders[i].OrderDate.ToString("yyyy-MM-dd");
                worksheet.Cell(i + 2,3).Value = orders[i].Customer?.CustomerName ?? "Unknown";
            }

            worksheet.Columns().AdjustToContents(); 

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(),"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","SalesOrders.xlsx");
        }
    }
}
