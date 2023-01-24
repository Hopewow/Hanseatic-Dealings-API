using Hanseatic_Dealings_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hanseatic_Dealings_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShipController : Controller
{
    private readonly DataContext _context;

    public ShipController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<ShipModel>>> Get()
    {

        return Ok(await _context.Ships.ToArrayAsync());
    }

    [HttpGet]
    [Route("GetPlayersShips")]
    public async Task<ActionResult<List<ShipModel>>> GetPlayersShips(int userId)
    {
        return Ok(await _context.Ships.Where(s => s.UserId == userId).ToArrayAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShipModel>> Get(int id)
    {
        var player = await _context.Ships.Include(c => c.Goods).FirstOrDefaultAsync(c => c.Id == id);
        if (player == null)
        {
            return BadRequest("Player not found.");
        }
        return Ok(player);
    } 

    [HttpPost]
    public async Task<ActionResult<ShipModel>> AddPlayer(ShipModel ship)
    {
        ship.Goods = new List<ShipStorageModel>();
        foreach (GoodsModel item in Enum.GetValues(typeof(GoodsModel)))
        {
            ShipStorageModel goods = new()
            {
                Amount = 0,
                Item = item
            };
            ship.Goods.Add(goods);
        }
       
        _context.Ships.Add(ship);
        await _context.SaveChangesAsync();
        
        return Ok( await _context.Ships.FindAsync(ship.Id));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveShip(int id)
    {
        var player = await _context.Ships.FindAsync(id);


        if (player == null)
        {
            return BadRequest("Player not found.");
        }

        _context.Ships.Remove(player);
        _context.SaveChanges();
        return Ok();
    }
}
