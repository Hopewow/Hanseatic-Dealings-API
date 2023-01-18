using Hanseatic_Dealings_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hanseatic_Dealings_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController : Controller
{
    private readonly DataContext _context;

    public PlayerController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<ShipModel>>> Get()
    {
        
        return Ok( await _context.Players.ToArrayAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShipModel>> Get(int id)
    {
        var player = await _context.Players.Include(c => c.Goods).FirstOrDefaultAsync(c => c.Id == id);
        if (player == null)
        {
            return BadRequest("Player not found.");
        }
        return Ok(player);
    } 

    [HttpPost]
    public async Task<ActionResult<List<ShipModel>>> AddPlayer(ShipModel ship)
    {
        _context.Players.Add(ship);
        await _context.SaveChangesAsync();
        return Ok( await _context.Players.ToListAsync());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveShip(int id)
    {
        var player = await _context.Players.FindAsync(id);


        if (player == null)
        {
            return BadRequest("Player not found.");
        }

        _context.Players.Remove(player);
        _context.SaveChanges();
        return Ok();
    }
}
