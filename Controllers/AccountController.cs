using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using auth_cs_gregslist.Models;
using auth_cs_gregslist.Services;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth_cs_gregslist.Controllers
{
  [ApiController]
  [Route("[controller]")]
  // TODO[epic=Auth] Adds authguard to all routes on the whole controller
  [Authorize]
  public class AccountController : ControllerBase
  {
    private readonly AccountsService _service;
    private readonly CarsService _carService;

    public AccountController(AccountsService service, CarsService carService)
    {
      _service = service;
      _carService = carService;
    }

    [HttpGet]
    public async Task<ActionResult<Account>> Get()
    {
      try
      {
        // TODO[epic=Auth] Replaces req.userinfo
        // IF YOU EVER NEED THE ACTIVE USERS INFO THIS IS HOW YOU DO IT (FROM AUTH0)
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        Account currentUser = _service.GetOrCreateAccount(userInfo);
        return Ok(currentUser);
      }
      catch (Exception error)
      {
        return BadRequest(error.Message);
      }
    }

    [HttpGet("cars")]
    public async Task<ActionResult<IEnumerable<Car>>> GetMyCars()
    {
      // TODO[epic=Auth] Replaces req.userinfo
      // IF YOU EVER NEED THE ACTIVE USERS INFO THIS IS HOW YOU DO IT (FROM AUTH0)
      Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
      IEnumerable<Car> cars = _carService.GetByCreatorId(userInfo.Id);
      return Ok(cars);

    }




  }
}