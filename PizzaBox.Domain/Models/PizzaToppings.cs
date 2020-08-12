using System.Collections.Generic;

namespace PizzaBox.Domain.Models
{

  public class PizzaToppings
  {

    public int ToppingId {get;set;}

    public ToppingModel Topping {get;set;}

    public int PizzaId {get;set;}
    public PizzaModel Pizza {get;set;}


  }

}