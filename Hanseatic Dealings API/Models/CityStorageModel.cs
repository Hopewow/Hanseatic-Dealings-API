using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Hanseatic_Dealings_API.Models;

public class CityStorageModel
{
    public int Id { get; set; }
    public GoodsModel Item { get; set; }
    public int Limit { get; set; }
    public int Current { get; set; }
    [JsonIgnore]
    public CityModel? City { get; set; }
    public int CityId { get; set; }
}
