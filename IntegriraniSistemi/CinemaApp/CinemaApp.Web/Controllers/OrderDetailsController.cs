using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.Identity;
using CinemaApp.Repository;
using CinemaApp.Service.Interface;
using GemBox.Document;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CinemaApp.Controllers
{
    public class OrderDetailsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<CinemaAppApplicationUser> _userManager;
        private readonly IUserOrderService _userOrderService;
        private readonly IOrderItemsService _orderItemsService;

        public OrderDetailsController(ApplicationDbContext context, UserManager<CinemaAppApplicationUser> userManager,IUserOrderService userOrderService, IOrderItemsService orderItemsService)
        {
            _context = context;
            _userManager = userManager;
            _userOrderService = userOrderService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            _orderItemsService = orderItemsService;
        }

        [HttpGet]
        [Route("/OrderDetails")]
        public IActionResult Index(int orderID)
        {

            ViewBag.OrderID = orderID;

            decimal totalAmount = _userOrderService.GetTotalAmountByOrderId(orderID);

            if (totalAmount != null)
            {
                ViewBag.TotalAmount = totalAmount;
            }
            else
            {
                // Handle the case when the order is not found
                return RedirectToAction("Index", "MyOrders");
            }


            List<(int OrderItemID, string MovieName, int Quantity, DateTime MovieDate, int Price)> orderItemDetails = _orderItemsService.GetOrderItemsDetails(orderID);
                

            ViewBag.OrderItems = orderItemDetails;

            return View(orderItemDetails);

        }


        public IActionResult GeneratePDF(int orderID)
        {
            var order = _context.UserOrder.FirstOrDefault(o => o.UserOrderID == orderID);
            if (order != null)
            {
                // Create a new DocumentModel object instead of Document
                var document = new DocumentModel();

                // Add content to the document
                var section = new GemBox.Document.Section(document);
                document.Sections.Add(section);

                // Order details
                section.Blocks.Add(new Paragraph(document, $"================================================================="));
                section.Blocks.Add(new Paragraph(document, $"Order ID: {order.UserOrderID}"));
                section.Blocks.Add(new Paragraph(document, $"Order Total Price: {order.TotalAmount}"));
                section.Blocks.Add(new Paragraph(document, $"Order Date of Making: {order.Date}"));
                section.Blocks.Add(new Paragraph(document, $"================================================================="));
                section.Blocks.Add(new Paragraph(document, $"Movies:"));


                // Order item details
                var orderItems = _context.OrderItem
                    .Where(oi => oi.UserOrderID == orderID)
                    .Join(
                        _context.MovieDates,
                        oi => oi.MovieDatesID,
                        md => md.MovieDatesID,
                        (oi, md) => new
                        {
                            OrderItem = oi,
                            MovieDate = md
                        })
                    .Join(
                        _context.Movie,
                        om => om.MovieDate.MovieID,
                        m => m.MovieID,
                        (om, m) => new
                        {
                            OrderItem = om.OrderItem,
                            MovieName = m.MovieName,
                            OrderItemDate = om.MovieDate.Date,
                            OrderItemPrice = om.OrderItem.Price,
                            OrderItemQuantity = om.OrderItem.Quantity
                        })
                    .ToList();

                foreach (var item in orderItems)
                {
                    section.Blocks.Add(new Paragraph(document, $"================================================================="));

                    var paragraph = new Paragraph(document);
                    paragraph.Inlines.Add(new Run(document, $"Movie Name: {item.MovieName}"));
                    paragraph.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                    paragraph.Inlines.Add(new Run(document, $"Movie Date: {item.OrderItemDate}"));
                    paragraph.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                    paragraph.Inlines.Add(new Run(document, $"Price per ticket: {item.OrderItemPrice}"));
                    paragraph.Inlines.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                    paragraph.Inlines.Add(new Run(document, $"Quantity: {item.OrderItemQuantity}"));

                    section.Blocks.Add(paragraph);

                    section.Blocks.Add(new Paragraph(document, $"================================================================="));
                }



                // Save the document to a memory stream
                var memoryStream = new MemoryStream();
                document.Save(memoryStream, SaveOptions.PdfDefault);

                // Set the position of the memory stream back to the beginning
                memoryStream.Position = 0;

                // Return the PDF file for download
                return File(memoryStream, "application/pdf", "Order.pdf");
            }

            // Handle the case when the order is not found
            return RedirectToAction("Index", "MyOrders");
        }



    }
}
