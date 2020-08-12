using System;
using PizzaBox.Domain.Models;
using PizzaBox.Storing;
using PizzaBox.Storing.Repositories;
using Xunit;

namespace PizzaBox.Testing
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

          
          PizzaModel pizza = new PizzaModel();

          pizza.Toppings.Add(new ToppingModel() {Name = "Pepperoni"});
          pizza.Crust = new CrustModel() {Name = "Stuffed"};
          pizza.Size = new SizeModel() {Name = "Medium"};
          pizza.Name = "Pepperoni";

          PizzaBoxRepository repo = new PizzaBoxRepository();

          repo.CreatePizza(pizza);

        }
    }
}
