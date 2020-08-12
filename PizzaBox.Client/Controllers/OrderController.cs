using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PizzaBox.Storing;
using PizzaBox.Domain.Models;
using PizzaBox.Client.Models;
using System;
using PizzaBox.Storing.Repositories;
using System.Collections.Generic;
using static PizzaBox.Client.Models.CartViewModel;
using System.Xml.Serialization;
using System.IO;

namespace PizzaBox.Client.Controllers
{

  public class OrderController : Controller
  {

    private readonly PizzaBoxDbContext _db;

    public OrderController(PizzaBoxDbContext dbContext){

      _db = dbContext;

    }

    [Route("/[controller]/[action]")]
    public IActionResult Edit(){

      PizzaBoxRepository repo = new PizzaBoxRepository();

      UserViewModelClient userViewModelClient = repo.InitUser(TempData["username"] as string,TempData["password"] as string);

      CartViewModel userViewModel = new CartViewModel() {
        Orders = userViewModelClient.Orders,
        User = userViewModelClient.User,
        Store = userViewModelClient.Store,
        //Cart = repo.CreateOrder()
      };

      List<CrustModel> crusts = _db.Crust.ToList();
      List<SizeModel> sizes = _db.Size.ToList();
      List<ToppingModel> toppings = _db.Topping.ToList();
      List<StoreModel> stores = _db.Store.ToList();

      userViewModel.SpecialtyPizzas = repo.GetSpecialtyPizzas();

      foreach (CrustModel crust in crusts){

        userViewModel.Crusts.Add(crust.Name);

      }

      foreach (SizeModel size in sizes){

        userViewModel.Sizes.Add(size.Name);

      }

      foreach (ToppingModel topping in toppings){

        userViewModel.Toppings.Add(topping.Name);

      }

      foreach (StoreModel store in stores){

        userViewModel.Stores.Add(store.Name);

      }

      /* userViewModel.AddToppingToList("Pepperoni");
      userViewModel.AddToppingToList("Bacon");
      userViewModel.AddToppingToList("Pineapple");

      userViewModel.AddSelectedTopping("Pepperoni");
      userViewModel.AddSelectedTopping("Bacon");
      userViewModel.AddSelectedTopping("Pineapple"); */

      OrderModel cart = new OrderModel();

      XmlSerializer serializer = new XmlSerializer(cart.GetType());
      using (StringWriter sw = new StringWriter()){
        
        serializer.Serialize(sw, cart);
        TempData["cart"] = sw.ToString();

      }

      userViewModel.Cart = cart;


      TempData["username"] = userViewModel.User.Login.Username;
      TempData["password"] = userViewModel.User.Login.Password;

      TempData.Keep();

      

      return View("Create", userViewModel);

    }

    [Route("/[controller]/[action]")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PlaceOrder(CartViewModel cartViewModel){

      PizzaBoxRepository repo = new PizzaBoxRepository();

      //.WriteLine("MY NAME IS " + TempData["username"] as string);

      UserViewModelClient userViewModelClient = repo.InitUser(TempData["username"] as string,TempData["password"] as string);

      cartViewModel.Orders = userViewModelClient.Orders;
      cartViewModel.User = userViewModelClient.User;
      cartViewModel.Store = userViewModelClient.Store;
      //cartViewModel.Cart = userViewModelClient.Cart;
      //cartViewModel.Cart.User = cartViewModel.User;

      List<CrustModel> crusts = _db.Crust.ToList();
      List<SizeModel> sizes = _db.Size.ToList();
      List<ToppingModel> toppings = _db.Topping.ToList();
      List<StoreModel> stores = _db.Store.ToList();
      cartViewModel.SpecialtyPizzas = repo.GetSpecialtyPizzas();

      foreach (CrustModel crust in crusts){

        cartViewModel.Crusts.Add(crust.Name);

      }

      foreach (SizeModel size in sizes){

        cartViewModel.Sizes.Add(size.Name);

      }

      foreach (ToppingModel topping in toppings){

        cartViewModel.Toppings.Add(topping.Name);

      }

      foreach (StoreModel store in stores){

        cartViewModel.Stores.Add(store.Name);

      }

      TempData["username"] = cartViewModel.User.Login.Username;
      TempData["password"] = cartViewModel.User.Login.Password;
      
      //Console.WriteLine("BUT MY REAL NAME IS " + cartViewModel.User.Name);

      string[] stringList = cartViewModel.SelectedToppings3.Split(' ');

      /* foreach (CheckBoxTopping check in cartViewModel.SelectedToppings2){
        
        if (check.isSelected){
          stringList.Add(check.Text);
        }

      } */

      /* Console.WriteLine("BOXES CHECKED: " + stringList.Count);
      Console.WriteLine("SELECTED TOPPINGS COUNT: " + cartViewModel.SelectedToppings2.Count); */

      OrderModel cart ;
      
      XmlSerializer deserializer = new XmlSerializer(typeof(OrderModel));
      using(TextReader tr = new StringReader(TempData["cart"] as string)){

        cart = (OrderModel)deserializer.Deserialize(tr);

      }

      if (ModelState.IsValid){

        /* repo.AddPizzaToCart(new PizzaModel() {Name = "Custom",
          Crust = repo.FindCrustByName(cartViewModel.Crust),
          Size = repo.FindSizeByName(cartViewModel.Size),
          Toppings = repo.FindToppingsByNames(stringList)
          },
          Int32.Parse(TempData["orderid"] as string)
          ); */

        cart.Pizzas.Add(new PizzaModel() {Name = "Custom",
          Crust = repo.FindCrustByName(cartViewModel.Crust),
          Size = repo.FindSizeByName(cartViewModel.Size),
          Toppings = repo.FindToppingsByNames(stringList)
          });

      }

      XmlSerializer serializer = new XmlSerializer(cart.GetType());
      using (StringWriter sw = new StringWriter()){
        
        serializer.Serialize(sw, cart);
        TempData["cart"] = sw.ToString();

      }

      cartViewModel.Cart = cart;

      //TempData["orderid"] = cartViewModel.Cart.Id.ToString();

      TempData.Keep();
      
      return View("Edit", cartViewModel);

    }

    [Route("/[controller]/[action]")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PlaceOrder2(CartViewModel cartView){

      PizzaBoxRepository repo = new PizzaBoxRepository();

      UserViewModelClient userViewModelClient = repo.InitUser(TempData["username"] as string,TempData["password"] as string);

      TempData["username"] = userViewModelClient.User.Login.Username;
      TempData["password"] = userViewModelClient.User.Login.Password;
      
      OrderModel cart ;
      
      XmlSerializer deserializer = new XmlSerializer(typeof(OrderModel));
      using(TextReader tr = new StringReader(TempData["cart"] as string)){

        cart = (OrderModel)deserializer.Deserialize(tr);

      }

      TempData.Keep();
      cart.Store = repo.FindStoreByName(cartView.StoreString);
      cart.PlaceOrder();
      cart.User = userViewModelClient.User;
      repo.PlaceOrder(cart);
      
      return View("AddedToCart");

    }

    [Route("/[controller]/[action]")]
    public IActionResult gotoIndex(){

      TempData["username"] = TempData["username"];
      TempData["password"] = TempData["password"];

      TempData.Keep();

      PizzaBoxRepository repo = new PizzaBoxRepository();

      UserViewModelClient userViewModelClient = repo.InitUser(TempData["username"] as string,TempData["password"] as string);

      UserViewModel userViewModel = new UserViewModel() {
        Orders = userViewModelClient.Orders,
        User = userViewModelClient.User,
        Store = userViewModelClient.Store,
        //Cart = repo.CreateOrder()
      };

      return View("Index", userViewModel);

    }

    [Route("/[controller]/[action]")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemovePizza(CartViewModel cartViewModel){

      PizzaBoxRepository repo = new PizzaBoxRepository();

      UserViewModelClient userViewModelClient = repo.InitUser(TempData["username"] as string,TempData["password"] as string);

      cartViewModel.Orders = userViewModelClient.Orders;
      cartViewModel.User = userViewModelClient.User;
      cartViewModel.Store = userViewModelClient.Store;

      List<CrustModel> crusts = _db.Crust.ToList();
      List<SizeModel> sizes = _db.Size.ToList();
      List<ToppingModel> toppings = _db.Topping.ToList();
      List<StoreModel> stores = _db.Store.ToList();
      cartViewModel.SpecialtyPizzas = repo.GetSpecialtyPizzas();

      foreach (CrustModel crust in crusts){

        cartViewModel.Crusts.Add(crust.Name);

      }

      foreach (SizeModel size in sizes){

        cartViewModel.Sizes.Add(size.Name);

      }

      foreach (ToppingModel topping in toppings){

        cartViewModel.Toppings.Add(topping.Name);

      }

      foreach (StoreModel store in stores){

        cartViewModel.Stores.Add(store.Name);

      }

      TempData["username"] = cartViewModel.User.Login.Username;
      TempData["password"] = cartViewModel.User.Login.Password;

      OrderModel cart ;
      
      XmlSerializer deserializer = new XmlSerializer(typeof(OrderModel));
      using(TextReader tr = new StringReader(TempData["cart"] as string)){

        cart = (OrderModel)deserializer.Deserialize(tr);

      }

      //Remove Pizza
      cart.Pizzas.RemoveAt(Int32.Parse(cartViewModel.PizzaNumber));

      XmlSerializer serializer = new XmlSerializer(cart.GetType());
      using (StringWriter sw = new StringWriter()){
        
        serializer.Serialize(sw, cart);
        TempData["cart"] = sw.ToString();

      }

      cartViewModel.Cart = cart;

      TempData.Keep();
      
      return View("Edit", cartViewModel);

    }

    [Route("/[controller]/[action]")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditPizza(CartViewModel cartViewModel){

      PizzaBoxRepository repo = new PizzaBoxRepository();

      UserViewModelClient userViewModelClient = repo.InitUser(TempData["username"] as string,TempData["password"] as string);

      cartViewModel.Orders = userViewModelClient.Orders;
      cartViewModel.User = userViewModelClient.User;
      cartViewModel.Store = userViewModelClient.Store;

      List<CrustModel> crusts = _db.Crust.ToList();
      List<SizeModel> sizes = _db.Size.ToList();
      List<ToppingModel> toppings = _db.Topping.ToList();
      List<StoreModel> stores = _db.Store.ToList();
      cartViewModel.SpecialtyPizzas = repo.GetSpecialtyPizzas();

      foreach (CrustModel crust in crusts){

        cartViewModel.Crusts.Add(crust.Name);

      }

      foreach (SizeModel size in sizes){

        cartViewModel.Sizes.Add(size.Name);

      }

      foreach (ToppingModel topping in toppings){

        cartViewModel.Toppings.Add(topping.Name);

      }

      foreach (StoreModel store in stores){

        cartViewModel.Stores.Add(store.Name);

      }

      TempData["username"] = cartViewModel.User.Login.Username;
      TempData["password"] = cartViewModel.User.Login.Password;

      OrderModel cart ;
      
      XmlSerializer deserializer = new XmlSerializer(typeof(OrderModel));
      using(TextReader tr = new StringReader(TempData["cart"] as string)){

        cart = (OrderModel)deserializer.Deserialize(tr);

      }

      string[] stringList = cartViewModel.SelectedToppings3.Split(' ');

      //Edit Pizza
      cart.Pizzas[Int32.Parse(cartViewModel.PizzaNumber)].Crust = repo.FindCrustByName(cartViewModel.Crust);
      cart.Pizzas[Int32.Parse(cartViewModel.PizzaNumber)].Size = repo.FindSizeByName(cartViewModel.Size);
      cart.Pizzas[Int32.Parse(cartViewModel.PizzaNumber)].Toppings = repo.FindToppingsByNames(stringList);
      
      
      /* cart.Pizzas.Add(new PizzaModel() {Name = "Custom",
          Crust = repo.FindCrustByName(cartViewModel.Crust),
          Size = repo.FindSizeByName(cartViewModel.Size),
          Toppings = repo.FindToppingsByNames(stringList)
          });
 */
      XmlSerializer serializer = new XmlSerializer(cart.GetType());
      using (StringWriter sw = new StringWriter()){
        
        serializer.Serialize(sw, cart);
        TempData["cart"] = sw.ToString();

      }

      cartViewModel.Cart = cart;

      TempData.Keep();
      
      return View("Edit", cartViewModel);

    }

    [Route("/[controller]/[action]")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddSpecialPizza(CartViewModel cartViewModel){

      PizzaBoxRepository repo = new PizzaBoxRepository();

      //.WriteLine("MY NAME IS " + TempData["username"] as string);

      UserViewModelClient userViewModelClient = repo.InitUser(TempData["username"] as string,TempData["password"] as string);

      cartViewModel.Orders = userViewModelClient.Orders;
      cartViewModel.User = userViewModelClient.User;
      cartViewModel.Store = userViewModelClient.Store;
      //cartViewModel.Cart = userViewModelClient.Cart;
      //cartViewModel.Cart.User = cartViewModel.User;

      List<CrustModel> crusts = _db.Crust.ToList();
      List<SizeModel> sizes = _db.Size.ToList();
      List<ToppingModel> toppings = _db.Topping.ToList();
      List<StoreModel> stores = _db.Store.ToList();

      cartViewModel.SpecialtyPizzas = repo.GetSpecialtyPizzas();

      foreach (CrustModel crust in crusts){

        cartViewModel.Crusts.Add(crust.Name);

      }

      foreach (SizeModel size in sizes){

        cartViewModel.Sizes.Add(size.Name);

      }

      foreach (ToppingModel topping in toppings){

        cartViewModel.Toppings.Add(topping.Name);

      }

      foreach (StoreModel store in stores){

        cartViewModel.Stores.Add(store.Name);

      }

      

      TempData["username"] = cartViewModel.User.Login.Username;
      TempData["password"] = cartViewModel.User.Login.Password;

      OrderModel cart ;
      
      XmlSerializer deserializer = new XmlSerializer(typeof(OrderModel));
      using(TextReader tr = new StringReader(TempData["cart"] as string)){

        cart = (OrderModel)deserializer.Deserialize(tr);

      }

      /* if (ModelState.IsValid){

        cart.Pizzas.Add(new PizzaModel() {Name = "Custom",
          Crust = repo.FindCrustByName(cartViewModel.Crust),
          Size = repo.FindSizeByName(cartViewModel.Size),
          Toppings = repo.FindToppingsByNames(stringList)
          });

      }
 */

      PizzaModel specialPizza = repo.GetSpecialPizzaByName(cartViewModel.SpecialtyPizza);
      cart.Pizzas.Add(specialPizza);

      XmlSerializer serializer = new XmlSerializer(cart.GetType());
      using (StringWriter sw = new StringWriter()){
        
        serializer.Serialize(sw, cart);
        TempData["cart"] = sw.ToString();

      }

      cartViewModel.Cart = cart;

      //TempData["orderid"] = cartViewModel.Cart.Id.ToString();

      TempData.Keep();
      
      return View("Edit", cartViewModel);

    }

    

  }

}