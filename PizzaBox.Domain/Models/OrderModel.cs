using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace PizzaBox.Domain.Models
{

  public class OrderModel : AModel
  {

    public UserModel User {get;set;}

    public StoreModel Store {get;set;}

    public DateTime dateOrdered {get;set;}

    public bool Placed {get;set;}

    public bool Completed {get;set;}
    
    [NotMapped]
    public List<PizzaModel> Pizzas {get;set;}

    public OrderModel(){

      Pizzas = new List<PizzaModel>();
      User = new UserModel();
      Store = new StoreModel();

    }

    public void PlaceOrder(){

      dateOrdered = DateTime.Now;
      Placed = true;

    }

    [XmlIgnore]
    public IList<OrderPizzas> OrderPizzas {get;set;}


  }

}