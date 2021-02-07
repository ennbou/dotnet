using System.Collections.Generic;
using System.Linq;
using examen.Models;
using Microsoft.AspNetCore.Mvc;

namespace examen.Controllers{
  public class Users : Controller{
    private readonly MyDbContext _db;

    public Users(MyDbContext db) {
      _db = db;
    }


    // GET
    public IActionResult Index() {
      IList<User> users = _db.Users.ToList();
      return View(users);
    }
  }
}