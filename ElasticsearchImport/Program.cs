using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElasticsearchImport.Projections;
using Nest;

namespace ElasticsearchImport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var client = new ElasticClient();
                client.DeleteIndex("orders");
                client.CreateIndex("orders", c => c
                .NumberOfReplicas(1)
                .NumberOfShards(6)
                .AddMapping<OrderIndex>(m => m.MapFromAttributes()));

                var descriptor = new BulkDescriptor();

                List<OrderIndex> toIndex = null;
                using (var db = new Northwind())
                {
                    toIndex = (from o in db.Orders
                        select new OrderIndex
                        {
                            CustomerId = o.CustomerID,
                            CustomerName = o.Customer.CompanyName,
                            OrderDate = o.OrderDate,
                            OrderId = o.OrderID,
                            OrderDetails = (from od in o.Order_Details select new OrderDetailIndex
                            {
                                Discount = od.Discount,
                                ProductId = od.ProductID,
                                ProductName = od.Product.ProductName,
                                Quantity = od.Quantity,
                                UnitPrice = od.UnitPrice
                            }).ToList() 
                        }).ToList();
                }
                foreach (var order in toIndex)
                    descriptor.Index<OrderIndex>(op => op.Index("orders").Document(order));

                var result = client.Bulk(d=>descriptor);
                Console.WriteLine("job's done");
            }
            catch (Exception exception)
            {

                Console.Write(exception);
            }

            Console.ReadKey();
        }
    }
}
