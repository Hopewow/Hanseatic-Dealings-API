namespace Hanseatic_Dealings_API.Models;

public class ShipModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Money { get; set; }
    public List<ShipStorageModel> Goods { get; set;}
}
