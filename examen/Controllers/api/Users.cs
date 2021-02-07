using System.Collections.Generic;
using examen.Models;
using Microsoft.AspNetCore.Mvc;

namespace examen.Controllers.api{
  [ApiController]
  [Route("api/[controller]")]
  public class Users : ControllerBase{
    private MyDbContext _db;

    public Users(MyDbContext db) {
      _db = db;
    }

    [HttpGet]
    public IEnumerable<User> GetAll() {
      return _db.Users;
    }
  }
}