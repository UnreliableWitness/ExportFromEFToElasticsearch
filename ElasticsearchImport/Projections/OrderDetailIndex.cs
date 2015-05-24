namespace ElasticsearchImport.Projections
{
    public class OrderDetailIndex
    {
        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public string ProductName { get; set; }

        public int ProductId { get; set; }
    }
}
