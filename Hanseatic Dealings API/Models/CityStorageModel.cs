namespace Hanseatic_Dealings_API.Models;

public class CityStorageModel
{
    public int Id { get; set; }
    public GoodsModel Item { get; set; }
    public int Limit { get; set; }
    public int Current { get; set; }
}
