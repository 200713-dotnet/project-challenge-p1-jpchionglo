using System.Collections.Generic;
using System.Xml.Serialization;

namespace PizzaBox.Domain.Models
{

  public class ToppingModel : AModel
  {

    public int Price {get;set;}


    [XmlIgnore]
    public IList<PizzaToppings> PizzaToppings {get;set;}

    public void IWantPizza()
    {

      var b = Brand.Instance();

      var p = b.PizzaFactory.Create();

    }

  }

}