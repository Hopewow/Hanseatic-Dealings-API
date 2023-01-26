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
        var city = await context.Cities.Include(c => c.Goods).FirstOrDefaultAsync(c => c.Id == id);
        if (city == null)
        {
            return BadRequest("City not found.");
        }

        return Ok(city);
    }

    [HttpPost]
    public async Task<ActionResult<List<CityModel>>> AddCity(CityModel city)
    {
        city.Goods = new List<CityStorageModel>();
        foreach (GoodsModel item in Enum.GetValues(typeof(GoodsModel)))
        {
            Random rnd = new();
            CityStorageModel good = new()
            {
                Item = item,
                Limit = rnd.Next(100, 200),
                Current = rnd.Next(0, 100),
            };
            city.Goods.Add(good);
        }

        context.Cities.Add(city);
        await context.SaveChangesAsync();

        return Ok(await context.Cities.ToListAsync());
    }

    [HttpPut]
    public async Task<ActionResult<List<CityModel>>> UpdateCity(CityModel city)
    {
        var dbCity = await context.Cities.FindAsync(city.Id);
        if (dbCity == null) return BadRequest("City does not exist.");

        dbCity.Name = city.Name;
        dbCity.Xcord = city.Xcord;
        dbCity.Ycord = city.Ycord;

        await context.SaveChangesAsync();

        return Ok(await context.Cities.ToListAsync());
    }

    [HttpPut("CityItem")]
    public async Task<ActionResult<CityModel>> UpdateCityItem(CityStorageModel storage)
    {
        var dbCityStorage = await context.CitiesStorages.FindAsync(storage.Id);
        if (dbCityStorage == null) return BadRequest("City does not exist.");

        dbCityStorage.Item = storage.Item;
        dbCityStorage.Limit = storage.Limit;
        dbCityStorage.Current = storage.Current;
        dbCityStorage.CityId = storage.CityId;

        await context.SaveChangesAsync();

        return Ok(await context.Cities.Include(c => c.Goods).FirstOrDefaultAsync(c => c.Id == storage.CityId));
    }

    [HttpPut("Purchase")]
    public async Task<ActionResult<CityModel>> PurchaseItem(MarketModel market)
    {
        var dbCityStorage = await context.CitiesStorages.FindAsync(market.CityStorage.Id);

        // Section for checking proper data is coming in for CityStorage
        if (dbCityStorage == null) return BadRequest("Item does not exist.");
        if (dbCityStorage.Current == 0) return NotFound("We seem to be out of this product.");
        if (dbCityStorage.Current != market.CityStorage.Current) return Conflict(await context.Cities.Include(c => c.Goods).FirstOrDefaultAsync(c => c.Id == market.CityStorage.CityId));

        // Section handles the transaction of how much product should be removed from the city and added to the player.
        int removeFromCurrent = (dbCityStorage.Current <= market.Amount) ? dbCityStorage.Current : market.Amount;

        var dbPlayerStorage = await context.ShipStorages.Where(p => (p.Item == market.CityStorage.Item && p.PlayerId == market.Player.Id)).FirstOrDefaultAsync();

        if (dbPlayerStorage == null)
        {
            var shipStorage = new ShipStorageModel
            {
                Item = market.CityStorage.Item,
                Amount = removeFromCurrent,
                PlayerId = market.Player.Id,
            };

            await context.ShipStorages.AddAsync(shipStorage);
        }
        
        dbPlayerStorage.Amount += removeFromCurrent;
        dbCityStorage.Current -= removeFromCurrent;

        // Section for getting price for material and checking if user has enough money.
        int pricePr = Convert.ToInt32(-3 * (2 * dbCityStorage.Current / dbCityStorage.Limit - 1) ^ 3 + 5);
        pricePr = pricePr * removeFromCurrent;

        var dbPlayer = await context.Ships.FindAsync(market.Player.Id);
        if (dbPlayer == null) return BadRequest("Player not found.");
        if (dbPlayer.Money <= pricePr) return StatusCode(StatusCodes.Status402PaymentRequired, "You do not have the funds for this transaction.");

        dbPlayer.Money -= (pricePr);

        await context.SaveChangesAsync();

        return Ok(await context.Cities.Include(c => c.Goods).FirstOrDefaultAsync(c => c.Id == market.CityStorage.CityId));
    }

    [HttpPut("Sell")]
    public async Task<ActionResult<CityModel>> SellItem(MarketModel market)
    {
        var dbCityStorage = await context.CitiesStorages.FindAsync(market.CityStorage.Id);

        // Section for checking proper data is coming in for CityStorage
        if (dbCityStorage == null) return BadRequest("Item does not exist.");
        if (dbCityStorage.Current >= dbCityStorage.Limit) return NotFound("We do not want your product.");
        if (dbCityStorage.Current != market.CityStorage.Current) return Conflict(await context.Cities.Include(c => c.Goods).FirstOrDefaultAsync(c => c.Id == market.CityStorage.CityId));

        // Section handles the transaction of how much product should be moved from the city from the player.
        var dbPlayerStorage = await context.ShipStorages.Where(p => (p.Item == market.CityStorage.Item && p.PlayerId == market.Player.Id)).FirstOrDefaultAsync();

        int removeFromCurrent = 0;

        if (dbPlayerStorage == null)
        {
            var shipStorage = new ShipStorageModel
            {
                Item = market.CityStorage.Item,
                Amount = 0,
                PlayerId = market.Player.Id,
            };

            await context.ShipStorages.AddAsync(shipStorage);
            return NotFound("You do not have this product on your ship.");
        }

        removeFromCurrent = (dbPlayerStorage.Amount <= market.Amount) ? dbPlayerStorage.Amount : market.Amount;
        int aboveLimit = removeFromCurrent + dbCityStorage.Current;
        if (aboveLimit > dbCityStorage.Limit) removeFromCurrent -= (aboveLimit - dbCityStorage.Limit);

        dbPlayerStorage.Amount -= removeFromCurrent;
        dbCityStorage.Current += removeFromCurrent;

        // Section for getting price for material and checking if user has enough money.
        int pricePr = Convert.ToInt32(-3 * (2 * dbCityStorage.Current / dbCityStorage.Limit - 1) ^ 3 + 3);

        var dbPlayer = await context.Ships.FindAsync(market.Player.Id);
        if (dbPlayer == null) return BadRequest("Player not found.");

        dbPlayer.Money += (pricePr*removeFromCurrent);

        await context.SaveChangesAsync();

        return Ok(await context.Cities.Include(c => c.Goods).FirstOrDefaultAsync(c => c.Id == market.CityStorage.CityId));
    }
}
