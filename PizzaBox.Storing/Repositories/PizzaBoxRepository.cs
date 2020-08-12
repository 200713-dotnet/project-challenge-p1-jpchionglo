using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PizzaBox.Client.Models;
using PizzaBox.Domain.Models;
using PizzaBox.Storing;

namespace PizzaBox.Storing.Repositories{


  public class PizzaBoxRepository{

    private PizzaBoxDbContext db = new PizzaBoxDbContext();

    //CREATE, READ, UPDATE, DELETE

    public void CreatePizza(PizzaModel pizza){

      pizza.Crust = db.Crust.FirstOrDefault(c => c.Name == pizza.Crust.Name);
      pizza.Size = db.Size.FirstOrDefault(s => s.Name == pizza.Size.Name);
      
      foreach (ToppingModel top in pizza.Toppings){

        ToppingModel top2 = db.Topping.FirstOrDefault(t => t.Name == top.Name);

        db.PizzaToppings.Add(new PizzaToppings() {Topping = top2, Pizza = pizza});
        

      }

      db.SaveChanges();

    }

    public OrderModel CreateOrder(){

      OrderModel order = new OrderModel();
      db.Order.Add(order);
      db.SaveChanges();
      order.Id = db.Order.Max(o => o.Id);
      return order;

    }

    public OrderModel PlaceOrder(OrderModel order){

      foreach (PizzaModel pizza1 in order.Pizzas){

        PizzaModel pizza;

        if (!pizza1.Name.Equals("Custom")){

          pizza = db.Pizza.FirstOrDefault(p => p.Name.Equals(pizza1.Name));

        } else {

          pizza = new PizzaModel();

          pizza.Crust = db.Crust.FirstOrDefault(c => c.Name == pizza1.Crust.Name);
          pizza.Size = db.Size.FirstOrDefault(s => s.Name == pizza1.Size.Name);

          foreach (ToppingModel topping in pizza1.Toppings){

            ToppingModel topping2 = db.Topping.FirstOrDefault(t => t.Name == topping.Name);
            db.PizzaToppings.Add(new PizzaToppings() {Pizza = pizza, Topping = topping2}); //Add to junction table

          }

        }

        db.OrderPizzas.Add(new OrderPizzas() {Order = order, Pizza = pizza}); //Add to junction table
        
      }

      db.SaveChanges();

      return order;

    }

    public StoreModel FindStoreByName(string name){

      return db.Store.FirstOrDefault(s => s.Name.Equals(name));

    }

    public OrderModel ReadUserCurrentOrder(UserModel user){

      return db.Order.FirstOrDefault(o => o.User.Id == user.Id && o.Placed == false); //finds the first order given user id

    }

    public List<OrderModel> ReadAllUserOrders(UserModel user){

      List<OrderModel> orders = new List<OrderModel>();

      foreach (OrderModel order in db.Order.ToList()){

        if (order.Completed == true && order.User.Id == user.Id){

          orders.Add(order);

        }

      }

      return orders;

    }

    public UserViewModelClient InitUser(string username, string password){

      UserViewModelClient User = new UserViewModelClient();

      int loginId = db.Login.FirstOrDefault(u => u.Username.Equals(username) && u.Password.Equals(password)).Id;

      User.User = db.User.FirstOrDefault(u => u.Login.Id == loginId);
      Console.WriteLine("INIT USER: " + User.User.Name);

      User.Orders = db.Order.Where(o => o.User.Id == User.User.Id).Include("Store").Include("User").ToList();

      //User.Orders = db.Order.Include(pizzas => db.Pizza.Include(db.Size.Include(db.Crust.Include(db.Topping))));

      /* List<OrderModel> cart = db.Order.Where(o => o.User.Id == User.User.Id && !o.Placed).ToList();
      

      if (cart.Count == 0){

        OrderModel neworder = new OrderModel();
        PlaceOrder(neworder);
        User.Cart = neworder;

      } else {

        List<PizzaModel> pizzas = new List<PizzaModel>();
        List<OrderPizzas> orderPizzas = db.OrderPizzas.Where(op => op.OrderId == cart[0].Id).Include("Pizza.Crust").Include("Pizza.Size").ToList();
        foreach (OrderPizzas orderPizza in orderPizzas){

          PizzaModel pizza = orderPizza.Pizza;
          List<ToppingModel> toppings = new List<ToppingModel>();
          List<PizzaToppings> pizzaToppings = db.PizzaToppings.Where(pt => pt.PizzaId == pizza.Id).Include("Topping").ToList();
          Console.WriteLine(pizzaToppings.Count);
          foreach (PizzaToppings topping in pizzaToppings){

            toppings.Add(topping.Topping);

          }

          pizza.Toppings = toppings;
          pizzas.Add(pizza);
          

        }
        
        User.Cart = cart[0];

        User.Cart.Pizzas = pizzas;
        Console.WriteLine("INIT USER PIZZA COUNT: " + pizzas.Count);

      } */

      foreach (OrderModel order in User.Orders){

        List<PizzaModel> pizzas = new List<PizzaModel>();
        List<OrderPizzas> orderPizzas = db.OrderPizzas.Where(op => op.OrderId == order.Id).Include("Pizza.Crust").Include("Pizza.Size").ToList();
        foreach (OrderPizzas orderPizza in orderPizzas){

          PizzaModel pizza = orderPizza.Pizza;
          List<ToppingModel> toppings = new List<ToppingModel>();
          List<PizzaToppings> pizzaToppings = db.PizzaToppings.Where(pt => pt.PizzaId == pizza.Id).Include("Topping").ToList();
          Console.WriteLine(pizzaToppings.Count);
          foreach (PizzaToppings topping in pizzaToppings){

            toppings.Add(topping.Topping);

          }

          pizza.Toppings = toppings;
          pizzas.Add(pizza);
          

        }
        order.Pizzas = pizzas;
        
        
      }

      return User;

    }

    public PizzaModel GetSpecialPizzaByName(string name){

      List<SpecialtyPizza> spizzas = db.SpecialtyPizzas.Where(p => p.Id != null).Include("Pizza.Crust").Include("Pizza.Size").ToList();

      PizzaModel pizza = spizzas.Where(p => p.Pizza.Name.Equals(name)).ToList()[0].Pizza;

      List<PizzaToppings> pizzaToppings = db.PizzaToppings.Where(pt => pt.PizzaId == pizza.Id).Include("Topping").ToList();
      foreach (PizzaToppings topping in pizzaToppings){

        pizza.Toppings.Add(topping.Topping);

      }
      return pizza;

    }

    public List<string> GetSpecialtyPizzas(){

      List<string> result = new List<string>();

      List<SpecialtyPizza> spizzas = db.SpecialtyPizzas.Where(p => p.Id != null).Include("Pizza").ToList();

      foreach (SpecialtyPizza pi in spizzas){

        result.Add(pi.Pizza.Name);

      }

      return result;
    }

    public UserViewModelClient InitUser(string username, string password, int orderid){

      UserViewModelClient User = new UserViewModelClient();

      int loginId = db.Login.FirstOrDefault(u => u.Username.Equals(username) && u.Password.Equals(password)).Id;

      User.User = db.User.FirstOrDefault(u => u.Login.Id == loginId);
      Console.WriteLine("INIT USER: " + User.User.Name);

      User.Orders = db.Order.Where(o => o.User.Id == User.User.Id).ToList();
      //User.Orders = db.Order.Include(pizzas => db.Pizza.Include(db.Size.Include(db.Crust.Include(db.Topping))));

      /* User.Cart = db.Order.FirstOrDefault(o => o.Id == orderid);

      List<PizzaModel> pizzas2 = new List<PizzaModel>();
      List<OrderPizzas> orderPizzas2 = db.OrderPizzas.Where(op => op.OrderId == orderid).Include("Pizza.Crust").Include("Pizza.Size").ToList();

      foreach (OrderPizzas orderPizza in orderPizzas2){

        PizzaModel pizza = orderPizza.Pizza;
        List<ToppingModel> toppings = new List<ToppingModel>();
        List<PizzaToppings> pizzaToppings = db.PizzaToppings.Where(pt => pt.PizzaId == pizza.Id).Include("Topping").ToList();
        Console.WriteLine(pizzaToppings.Count);
        foreach (PizzaToppings topping in pizzaToppings){

          toppings.Add(topping.Topping);

        }

        pizza.Toppings = toppings;
        pizzas2.Add(pizza);
          

      }
      User.Cart.Pizzas = pizzas2;
      db.SaveChanges(); */


      //Console.WriteLine("INIT USER NUMBER IN CART: " + User.Cart.Pizzas.Count);

      foreach (OrderModel order in User.Orders){

        List<PizzaModel> pizzas = new List<PizzaModel>();
        List<OrderPizzas> orderPizzas = db.OrderPizzas.Where(op => op.OrderId == order.Id).Include("Pizza.Crust").Include("Pizza.Size").ToList();
        foreach (OrderPizzas orderPizza in orderPizzas){

          PizzaModel pizza = orderPizza.Pizza;
          List<ToppingModel> toppings = new List<ToppingModel>();
          List<PizzaToppings> pizzaToppings = db.PizzaToppings.Where(pt => pt.PizzaId == pizza.Id).Include("Topping").ToList();
          Console.WriteLine(pizzaToppings.Count);
          foreach (PizzaToppings topping in pizzaToppings){

            toppings.Add(topping.Topping);

          }

          pizza.Toppings = toppings;
          pizzas.Add(pizza);
          

        }
        order.Pizzas = pizzas;
        
        
      }

      return User;

    }

    public CrustModel FindCrustByName(string name){

      return db.Crust.FirstOrDefault(c => c.Name == name);

    }
    public SizeModel FindSizeByName(string name){

      return db.Size.FirstOrDefault(s => s.Name == name);

    }

    public List<ToppingModel> FindToppingsByNames(string[] names){

      List<ToppingModel> toppings = new List<ToppingModel>();

      foreach (string topping in names){

        toppings.Add(db.Topping.FirstOrDefault(t => t.Name == topping));

      }

      return toppings;

    }

    /* public void UpdateCart(OrderModel cart, int oid){

      List<OrderModel> orders = db.Order.Where(o => o.Placed == false).ToList();

      Console.WriteLine("UPDATECART CARTCOUNT: " + cart.Pizzas.Count);

      if (orders.Count == 0){

        Console.WriteLine("PIZZAS IN CART: " + cart.Pizzas.Count);
        PlaceOrder(cart);
        foreach(PizzaModel pizza in cart.Pizzas){

          if (db.OrderPizzas.Where(p => p.PizzaId == pizza.Id).ToList().Count == 0){ //If order not in junction table, add order to junction table

            db.OrderPizzas.Add(new OrderPizzas() {Pizza = pizza, Order = cart});

          } 

        }
        
        db.SaveChanges();

      } else {

        for (int i = 0; i < cart.Pizzas.Count - orders[0].Pizzas.Count; i++){
          orders[0].Pizzas.Add(cart.Pizzas[cart.Pizzas.Count + i]);
        }
        
      }

    } */

    public void AddPizzaToCart(PizzaModel pizza, int orderId){

      OrderModel cart = db.Order.FirstOrDefault(o => o.Id == orderId);

      List<PizzaModel> pizzas2 = new List<PizzaModel>();
      List<OrderPizzas> orderPizzas2 = db.OrderPizzas.Where(op => op.OrderId == orderId).Include("Pizza.Crust").Include("Pizza.Size").ToList();

      foreach (OrderPizzas orderPizza in orderPizzas2){

        PizzaModel pizza3 = orderPizza.Pizza;
        List<ToppingModel> toppings = new List<ToppingModel>();
        List<PizzaToppings> pizzaToppings = db.PizzaToppings.Where(pt => pt.PizzaId == pizza3.Id).Include("Topping").ToList();
        Console.WriteLine(pizzaToppings.Count);
        foreach (PizzaToppings topping in pizzaToppings){

          toppings.Add(topping.Topping);

        }

        pizza3.Toppings = toppings;
        pizzas2.Add(pizza3);
          

      }
      cart.Pizzas = pizzas2;
      db.SaveChanges();

      Console.WriteLine("Add Pizza to cart before add: " + cart.Pizzas.Count);
      cart.Pizzas.Add(pizza);
      Console.WriteLine("Add Pizza to cart after add: " + cart.Pizzas.Count);
      db.SaveChanges();

    }

    public UserModel FindUserByID(int id){

      return db.User.FirstOrDefault(u => u.Id == id);

    }

    public void Update(){

      

    }

    //public void Delete();


  }


}