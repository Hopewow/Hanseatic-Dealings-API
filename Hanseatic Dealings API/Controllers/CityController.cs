using Hanseatic_Dealings_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using NuGet.Versioning;

namespace Hanseatic_Dealings_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly DataContext context;

    public CityController(DataContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<CityModel>>> Get()
    {
        return Ok(await context.Cities.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CityModel>> Get(int id)
    {
        CityModel city = await context.Cities.Include(c => c.Goods).FirstOrDefaultAsync(c => c.Id == id);
        if (city == null)
        {
            return BadRequest("City not found.");
        }

        return Ok(city);
    }

    [HttpPost]
    public async Task<ActionResult<List<CityModel>>> AddCity (CityModel city)
    {
        context.Cities.Add(city);
        await context.SaveChangesAsync();

        return Ok(await context.Cities.ToListAsync());
    }
}
