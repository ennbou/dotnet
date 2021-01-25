using System.ComponentModel.DataAnnotations.Schema;

namespace apprest{
  public class Client{
    public int Id {  get; set;}
    public string Name {  get; set; }
    public string Email {  get; set;}
  }
}