using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Confluent.Kafka;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apprest.Controllers{
  [ApiController]
  [Route("[controller]")]
  public class Products : ControllerBase{
    private MyDbContext _db;

    private IProducer<string, string> producer;

    public Products(MyDbContext db) {
      _db = db;
      producer = new ProducerBuilder<string, string>(new ProducerConfig {
        BootstrapServers = "localhost:9092",
        ClientId = Dns.GetHostName()
      }).Build();
    }

    public void Send(string value) {
      producer.ProduceAsync("dotnet", new Message<string, string> {Key = "Products", Value = value});
    }

    [HttpGet]
    public IEnumerable<Product> GetAll() {
      return _db.Products.Include(p => p.Categorie);
    }

    [HttpGet("{id}")]
    public Product GetById(int id) {
      return _db.Products.FirstOrDefault(c => c.Id == id);
    }

    [HttpPost]
    public Product Save([FromBody] Product product) {
      _db.Products.Add(product);
      _db.SaveChanges();
      Send($"save [id:{product.Id + ""}, name:{product.Name}, cId:{product.CategorieId + ""}]");
      return product;
    }

    [HttpDelete("{id}")]
    public void DeleteById(int id) {
      Product product = _db.Products.FirstOrDefault(c => c.Id == id);
      _db.Products.Remove(product);
      _db.SaveChanges();
      Send($"delete [id:{product.Id + ""}, name:{product.Name}, cId:{product.CategorieId + ""}]");
    }

    [HttpPut("{id}")]
    public Product Update(int id, [FromBody] Product product) {
      product.Id = id;
      _db.Products.Update(product);
      _db.SaveChanges();
      Send($"update [id:{product.Id + ""}, name:{product.Name}, cId:{product.CategorieId + ""}]");
      return product;
    }
  }
}