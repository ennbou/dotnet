using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace apprest{
  public class Categorie{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public virtual List<Product> Products { get; set; }
  }
}