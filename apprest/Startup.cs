using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace apprest{
  public class Startup{
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
public void ConfigureServices(IServiceCollection services) {
  services.AddCors(o => o.AddPolicy("mp", builder => {
    builder.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader();
  }));
  services.AddControllers();
  services.AddDbContext<MyDbContext>(options => { options.UseInMemoryDatabase("MyDb1"); });
}

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MyDbContext _db) {
  if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
  app.UseHttpsRedirection();
  app.UseRouting();
  app.UseAuthorization();
  app.UseCors("mp");
  app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
  _db.Clients.Add(new Client {Name= "client1",Email = "email1@ennbou.com"});
  _db.Categories.Add(new Categorie {Name = "category 1"});
  _db.Categories.Add(new Categorie {Name = "category 2"});
  _db.Products.Add(new Product {Name = "product 1", Price = 99.0, CategorieId = 1});
  _db.Products.Add(new Product {Name = "product 5", Price = 99.0, CategorieId = 2});
  _db.SaveChanges();
}
  }

public class MyDbContext : DbContext{
  public MyDbContext(DbContextOptions options) : base(options) {
  }
  public DbSet<Client> Clients { get; set; }
  public DbSet<Product> Products { get; set; }
  public DbSet<Categorie> Categories { get; set; }
}
}