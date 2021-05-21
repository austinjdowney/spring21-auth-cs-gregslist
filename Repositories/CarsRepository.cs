using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using auth_cs_gregslist.Models;
using Dapper;

namespace auth_cs_gregslist.Repositories
{
  public class CarsRepository
  {
    private readonly IDbConnection _db;

    public CarsRepository(IDbConnection db)
    {
      _db = db;
    }

    public IEnumerable<Car> GetAll()
    {
      string sql = @"
      SELECT
       c.*,
       a.*
      FROM cars c
      JOIN accounts a ON c.creatorId = a.id;";
      return _db.Query<Car, Account, Car>(sql, (car, account) =>
      {
        car.Creator = account;
        return car;
      }, splitOn: "id");
    }

    public IEnumerable<Car> GetByCreatorId(string id)
    {
      string sql = @"
      SELECT 
        c.*,
        a.* 
      FROM cars c
      JOIN accounts a ON c.creatorId = a.id
      WHERE creatorId = @id";
      return _db.Query<Car, Account, Car>(sql, (car, account) =>
      {
        car.Creator = account;
        return car;
      }
      , new { id }, splitOn: "id");
    }
    public Car GetById(int id)
    {
      string sql = @"
      SELECT 
        c.*,
        a.* 
      FROM cars c
      JOIN accounts a ON c.creatorId = a.id
      WHERE id = @id";
      return _db.Query<Car, Account, Car>(sql, (car, account) =>
      {
        car.Creator = account;
        return car;
      }
      , new { id }, splitOn: "id").FirstOrDefault();
    }

    public Car Create(Car newCar)
    {
      string sql = @"
      INSERT INTO cars
      (creatorId, make, model, year, price, imgUrl, description)
      VALUES
      (@CreatorId, @Make, @Model, @Year, @Price, @ImgUrl, @Description);
      SELECT LAST_INSERT_ID()";
      newCar.Id = _db.ExecuteScalar<int>(sql, newCar);
      return newCar;
    }

    public bool Delete(int id)
    {
      string sql = "DELETE FROM cars WHERE id = @id LIMIT 1";
      int affectedRows = _db.Execute(sql, new { id });
      return affectedRows == 1;
    }
  }
}