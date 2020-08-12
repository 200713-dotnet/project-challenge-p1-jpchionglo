using System;
using PizzaBox.Client.Models;
using PizzaBox.Domain.Models;
using PizzaBox.Storing;
using PizzaBox.Storing.Repositories;
using Xunit;

namespace PizzaBox.Testing
{
    public class PlaceOrderTest
    {
        [Theory]
        [InlineData("Pepperoni","HandTossed","Medium","Pepperoni")]
        //[InlineData("Pepperoni","HandTossed","Medium","Pepperoni")]
        //[InlineData("Pepperoni","HandTossed","Medium","Pepperoni")]
        public void Test1(string topping, string crust, string size, string name)
        {

          

          PizzaModel pizza = new PizzaModel();

          pizza.Toppings.Add(new ToppingModel() {Name = topping});
          pizza.Crust = new CrustModel() {Name = crust};
          pizza.Size = new SizeModel() {Name = size};
          pizza.Name = name;

          PizzaBoxRepository repo = new PizzaBoxRepository();

          UserViewModelClient user = repo.InitUser("jpchionglo","password");
          
          StoreModel store = repo.FindStoreByName("Oakwood");

          OrderModel order = new OrderModel() {

            Pizzas = new System.Collections.Generic.List<PizzaModel>() { pizza },
            User = user.User,
            Store = store

          };

          order.PlaceOrder();

          repo.PlaceOrder(order); 

        }
    }
}
