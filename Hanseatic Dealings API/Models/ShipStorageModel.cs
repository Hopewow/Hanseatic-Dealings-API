namespace Hanseatic_Dealings_API.Models
{
    public class ShipStorageModel
    {
        public int Id { get; set; }
        public GoodsModel Item { get; set; }
        public int Amount { get; set; }
    }
}
