using System.ComponentModel.DataAnnotations.Schema;

namespace apprest{
  public class Product{
    public int Id { set; get; }
    public string Name { set; get; }
    public double Price { set; get; }
    
    public int CategorieId { get; set;  }
    
    [ForeignKey("CategorieId")]
    public virtual Categorie Categorie { get; set; }
  }
}