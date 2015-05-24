using System;
using System.Collections.Generic;
using Nest;

namespace ElasticsearchImport.Projections
{
    
    [ElasticType(Name="OrderIndexType", IdProperty = "OrderId")]
    public class OrderIndex
    {
        public int OrderId { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public DateTime? OrderDate { get; set; }

        public List<OrderDetailIndex> OrderDetails { get; set; }
    }
}
