using System;
using System.Collections.Generic;
using System.Data;
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

    internal IEnumerable<Car> GetAll()
    {
      string sql = "SELECT * FROM cars";
      return _db.Query<Car>(sql);
    }

    internal IEnumerable<Car> GetByCreatorId(string id)
    {
      string sql = "SELECT * FROM cars WHERE creatorId = @id";
      return _db.Query<Car>(sql, new { id });
    }
    internal Car GetById(int id)
    {
      string sql = "SELECT * FROM cars WHERE id = @id";
      return _db.QueryFirstOrDefault<Car>(sql, new { id });
    }

    internal Car Create(Car newCar)
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

    internal bool Delete(int id)
    {
      string sql = "DELETE FROM cars WHERE id = @id LIMIT 1";
      int affectedRows = _db.Execute(sql, new { id });
      return affectedRows == 1;
    }
  }
}