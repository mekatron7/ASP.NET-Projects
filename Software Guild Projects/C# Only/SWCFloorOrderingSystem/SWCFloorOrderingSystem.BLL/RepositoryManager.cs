using SWCFloorOrderingSystem.Models;
using SWCFloorOrderingSystem.Models.Interfaces;
using SWCFloorOrderingSystem.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCFloorOrderingSystem.BLL
{
    public class RepositoryManager
    {
        private IOrderRepository _orderRepository;

        public RepositoryManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public OrdersOnDateLookupResponse LookupOrdersOnDate(DateTime date)
        {
            OrdersOnDateLookupResponse response = new OrdersOnDateLookupResponse();

            response.Orders = _orderRepository.LoadAllOrders(date);

            if(response.Orders == null)
            {
                response.Success = false;
                response.Message = $"There are no orders on record for {date.ToShortDateString()}.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public SingleOrderLookupResponse LookupOrder(DateTime date, int orderNumber)
        {
            SingleOrderLookupResponse response = new SingleOrderLookupResponse();

            response.Order = _orderRepository.LoadOrder(date, orderNumber);

            if(response.Order == null)
            {
                response.Success = false;
                response.Message = $"There is no order with an order number of {orderNumber} on record for {date.ToShortDateString()}.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public AddOrderResponse AddOrder(DateTime date, string name, string state, string productType, decimal area)
        {
            AddOrderResponse response = new AddOrderResponse();

            ITaxInfo getTaxes = TaxInfoFactory.Create();
            List<TaxInfo> taxInfo = getTaxes.GetTaxInfo();
            IProductInfo products = ProductInfoFactory.Create();
            List<ProductInfo> productInfo = products.GetProducts();

            if(date < DateTime.Now)
            {
                response.Success = false;
                response.Message = "You can't place an order on a past date.\nYou can't change the past, but you can change the future.";
                return response;
            }

            if(taxInfo.SingleOrDefault(i => i.StateAbbr == state) == null)
            {
                response.Success = false;
                response.Message = $"Sorry, we aren't able to place orders in {state}.";
                return response;
            }

            if(productInfo.SingleOrDefault(i => i.ProductType == productType) == null)
            {
                response.Success = false;
                response.Message = $"Sorry, {productType} isn't a valid product type we offer.";
                return response;
            }

            if(area < 100)
            {
                response.Success = false;
                response.Message = "The area must be at least 100 sq ft.";
                return response;
            }


            //Creates the new order if everthing is valid
            Order newOrder = new Order
            {
                OrderDate = date,
                CustomerName = name,
                State = state,
                ProductType = productType,
                Area = area,
                TaxRate = taxInfo.Single(i => i.StateAbbr == state).TaxRate,
                CostPerSquareFoot = productInfo.Single(i => i.ProductType == productType).CostPerSqFt,
                LaborCostPerSquareFoot = productInfo.Single(i => i.ProductType == productType).LaborCostPerSqFt
            };

            response.Success = true;
            response.NewOrder = newOrder;
            response.OrderRepo = _orderRepository;
            return response;
        }

        public EditOrderResponse EditOrder(Order order)
        {
            EditOrderResponse response = new EditOrderResponse();

            ITaxInfo getTaxes = TaxInfoFactory.Create();
            List<TaxInfo> taxInfo = getTaxes.GetTaxInfo();
            IProductInfo products = ProductInfoFactory.Create();
            List<ProductInfo> productInfo = products.GetProducts();

            if (taxInfo.SingleOrDefault(i => i.StateAbbr == order.State) == null)
            {
                response.Success = false;
                response.Message = $"Sorry, we aren't able to place orders in {order.State}.";
                return response;
            }

            if (productInfo.SingleOrDefault(i => i.ProductType == order.ProductType) == null)
            {
                response.Success = false;
                response.Message = $"Sorry, {order.ProductType} isn't a valid product type we offer.";
                return response;
            }

            if (order.Area < 100)
            {
                response.Success = false;
                response.Message = "The area must be at least 100 sq ft.";
                return response;
            }

            Order newOrder = new Order
            {
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                CustomerName = order.CustomerName,
                State = order.State,
                TaxRate = taxInfo.Single(i => i.StateAbbr == order.State).TaxRate,
                ProductType = order.ProductType,
                Area = order.Area,
                CostPerSquareFoot = productInfo.Single(i => i.ProductType == order.ProductType).CostPerSqFt,
                LaborCostPerSquareFoot = productInfo.Single(i => i.ProductType == order.ProductType).LaborCostPerSqFt
            };

            response.Success = true;
            response.OrderEdit = newOrder;
            response.OrderRepo = _orderRepository;
            return response;
        }

        public DeleteOrderResponse DeleteOrder(Order order)
        {
            DeleteOrderResponse response = new DeleteOrderResponse();

            response.Success = true;
            response.OrderToDelete = order;
            response.Message = "Your order has been successfully deleted.";

            _orderRepository.DeleteOrder(order);

            return response;
        }
    }
}
