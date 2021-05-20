using System;
using System.Collections.Generic;
using auth_cs_gregslist.Models;
using auth_cs_gregslist.Repositories;

namespace auth_cs_gregslist.Services
{
  public class CarsService
  {
    private readonly CarsRepository _repo;

    public CarsService(CarsRepository repo)
    {
      _repo = repo;
    }

    internal IEnumerable<Car> GetAll()
    {
      return _repo.GetAll();
    }

    internal Car GetById(int id)
    {
      Car car = _repo.GetById(id);
      if (car == null)
      {
        throw new Exception("Invalid Car Id");
      }
      return car;
    }

    internal IEnumerable<Car> GetByCreatorId(string id)
    {
      return _repo.GetByCreatorId(id);
    }



    internal Car Create(Car newCar)
    {
      return _repo.Create(newCar);
    }

    internal void Delete(int id, string creatorId)
    {
      Car car = GetById(id);
      if (car.CreatorId != creatorId)
      {
        throw new Exception("You cannot delete another users Car");
      }
      if (!_repo.Delete(id))
      {
        throw new Exception("Something has gone terribly wrong");
      };
    }
  }
}