using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace PizzaBox.Domain.Models
{

  public class PizzaModel : AModel
  {

    public CrustModel Crust {get;set;}
    public SizeModel Size {get;set;}
    int Price {get;set;}

    [NotMapped]
    public List<ToppingModel> Toppings {get;set;}

    [XmlIgnore]
    public IList<PizzaToppings> PizzaToppings {get;set;}

    [XmlIgnore]
    public IList<OrderPizzas> OrderPizzas {get;set;}

    public PizzaModel(){

      Name = "Custom";
      Toppings = new List<ToppingModel>();

    }

    

  }

}