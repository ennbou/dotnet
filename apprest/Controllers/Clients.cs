using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using System.Net;

namespace apprest.Controllers{
  [ApiController]
  [Route("[controller]")]
  public class Clients : ControllerBase{
    private MyDbContext _db;

    private IProducer<string, string> producer;
    public Clients(MyDbContext db) {
      _db = db;
      producer = new ProducerBuilder<string, string>(new ProducerConfig {
        BootstrapServers = "localhost:9092",
        ClientId = Dns.GetHostName()
      }).Build();
    }
    public void Send( string value) {
      producer.ProduceAsync("dotnet", new Message<string, string> {Key = "Client", Value = value});
    }

[HttpGet]
public IEnumerable<Client> GetAll() {
  return _db.Clients;
}

[HttpGet("{id}")]
public Client GetById(int id) {
  return _db.Clients.FirstOrDefault(c => c.Id == id);
}

[HttpPost]
public Client Save([FromBody] Client client) {
  _db.Clients.Add(client);
  _db.SaveChanges();
  Send( $"save [id:{client.Id + ""}, name:{client.Name}, email:{client.Email}]");
  return client;
}

[HttpDelete("{id}")]
public void DeleteById(int id) {
  var client = _db.Clients.FirstOrDefault(c => c.Id == id);
  _db.Clients.Remove(client);
  _db.SaveChanges();
  Send( $"delete [id:{client.Id + ""}, name:{client.Name}, email:{client.Email}]");
}

[HttpPut("{id}")]
public Client Update(int id, [FromBody] Client client) {
  client.Id = id;
  _db.Clients.Update(client);
  _db.SaveChanges();
  Send( $"save [id:{client.Id + ""}, name:{client.Name}, email:{client.Email}]");
  return client;
}
  }
}