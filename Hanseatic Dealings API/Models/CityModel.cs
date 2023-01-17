using System.Text.Json.Serialization;

namespace Hanseatic_Dealings_API.Models;

public class CityModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Xcord { get; set; }
    public string Ycord { get; set; }
    public List<CityStorageModel> Goods { get; set; }
}
