using System.Collections.Generic;

namespace PizzaBox.Domain.Models
{

  public class OrderPizzas
  {

    public int PizzaOrderId {get;set;}
    
    public int OrderId {get;set;}

    public OrderModel Order {get;set;}

    public int PizzaId {get;set;}
    public PizzaModel Pizza {get;set;}


  }

}