using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apprest.Controllers{
  [ApiController]
  [Route("[controller]")]
  public class Categories : ControllerBase{
    private MyDbContext _db;
    private IProducer<string, string> producer;

    public Categories(MyDbContext db) {
      _db = db;
      producer = new ProducerBuilder<string, string>(new ProducerConfig {
        BootstrapServers = "localhost:9092",
        ClientId = Dns.GetHostName()
      }).Build();
    }

    public void Send(string value) {
      producer.ProduceAsync("dotnet", new Message<string, string> {Key = "Categorie", Value = value});
    }

    [HttpGet]
    public IEnumerable<Categorie> GetAll() {
      return _db.Categories;
    }

    [HttpGet("{id}")]
    public Categorie GetById(int id) {
      return _db.Categories.FirstOrDefault(c => c.Id == id);
    }

    [HttpGet("{id}/products")]
    public IEnumerable<Product> GetProdsById(int id) {
      return _db.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == id).Products;
    }

    [HttpPost]
    public Categorie Save([FromBody] Categorie categorie) {
      _db.Categories.Add(categorie);
      _db.SaveChanges();
      Send($"save [id:{categorie.Id + ""}, name:{categorie.Name}]");
      return categorie;
    }

    [HttpDelete("{id}")]
    public void DeleteById(int id) {
      Categorie categorie = _db.Categories.FirstOrDefault(c => c.Id == id);
      _db.Categories.Remove(categorie);
      _db.SaveChanges();
      Send($"delete [id:{categorie.Id + ""}, name:{categorie.Name}]");
    }

    [HttpPut("{id}")]
    public Categorie Update(int id, [FromBody] Categorie categorie) {
      categorie.Id = id;
      _db.Categories.Update(categorie);
      _db.SaveChanges();
      Send($"update [id:{categorie.Id + ""}, name:{categorie.Name}]");
      return categorie;
    }
  }
}