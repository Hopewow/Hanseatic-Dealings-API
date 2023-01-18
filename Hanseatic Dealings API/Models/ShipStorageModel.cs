using System.Text.Json.Serialization;

namespace Hanseatic_Dealings_API.Models
{
    public class ShipStorageModel
    {
        public int Id { get; set; }
        public GoodsModel Item { get; set; }
        public int Amount { get; set; }
        [JsonIgnore]
        public ShipModel? Player { get; set; }
        public int PlayerId { get; set; }
    }
}
