using Hanseatic_Dealings_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hanseatic_Dealings_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private DataContext context;

    public UserController(DataContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<UserModel>> Get(string email, string psw)
    {
        var user = await context.Users.Where(u => u.email == email && u.password == psw).FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound("User does not exist.");
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserModel>> AddUser(UserModel usr)
    {
        context.Users.Add(usr);
        await context.SaveChangesAsync();

        return Ok("Created");
    }
}
