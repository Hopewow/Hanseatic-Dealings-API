using Hanseatic_Dealings_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hanseatic_Dealings_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController : Controller
{
    private static List<ShipModel> players = new List<ShipModel>
    {
        new ShipModel {
            Id = 1,
            Name = "Kamakazi",
            Money = 100,
            Goods = new List<ShipStorageModel>
            {
                new ShipStorageModel
                {
                     Id = 1,
                     Item = GoodsModel.Wood,
                     Amount = 13,
                },
                new ShipStorageModel
                {
                    Id = 2,
                    Item = GoodsModel.Meat,
                    Amount = 4,
                }
            }
        },
        new ShipModel {
            Id = 2,
            Name = "Yamato",
            Money = 100,
            Goods = new List<ShipStorageModel>
            {
                new ShipStorageModel
                {
                    Id = 3,
                    Item = GoodsModel.Beer,
                    Amount = 34,
                },
                new ShipStorageModel
                {
                    Id = 4,
                    Item = GoodsModel.Iron,
                    Amount = 23,
                }
            }
        }
    };

    [HttpGet]
    public async Task<ActionResult<List<ShipModel>>> Get()
    {
        return Ok(players);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShipModel>> Get(int id)
    {
        ShipModel player = players.Find(x => x.Id == id);
        if (player == null)
        {
            return BadRequest("Player not found.");
        }
        return Ok(player);
    } 

    [HttpPost]
    public async Task<ActionResult<List<ShipModel>>> AddPlayer(ShipModel ship)
    {
        players.Add(ship);
        return Ok(players);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> removeShip(int id)
    {
        ShipModel player = players.Find(x => x.Id == id);

        if (player == null)
        {
            return BadRequest("Player not found.");
        }

        players.Remove(player);
        return Ok();
    }
}
