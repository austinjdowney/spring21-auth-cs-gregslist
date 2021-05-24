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
  [Route("api/[controller]")]
  public class CarsController : ControllerBase
  {
    private readonly CarsService _service;
    private readonly AccountsService _acctService;

    public CarsController(CarsService service, AccountsService acctsService)
    {
      _service = service;
      _acctService = acctsService;
    }


    [HttpGet]
    public ActionResult<IEnumerable<Car>> GetAll()
    {
      try
      {
        IEnumerable<Car> cars = _service.GetAll();
        return Ok(cars);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }


    [HttpGet("{id}")]
    public ActionResult<Car> GetById(int id)
    {
      try
      {
        Car found = _service.GetById(id);
        return Ok(found);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }



    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Car>> Create([FromBody] Car newCar)
    {
      try
      {
        // TODO[epic=Auth] Get the user info to set the creatorID
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        // safety to make sure an account exists for that user before CREATE-ing stuff.
        Account fullAccount = _acctService.GetOrCreateAccount(userInfo);
        newCar.CreatorId = userInfo.Id;

        Car car = _service.Create(newCar);
        //TODO[epic=Populate] adds the account to the new object as the creator
        car.Creator = fullAccount;
        return Ok(car);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }


    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<Car>> Delete(int id)
    {
      try
      {
        // TODO[epic=Auth] Get the user info to set the creatorID
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        // safety to make sure an account exists for that user before CREATE-ing stuff.
        _service.Delete(id, userInfo.Id);
        return Ok("Delorted");
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}