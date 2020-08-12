using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PizzaBox.Client.Models;
using PizzaBox.Domain.Models;
using PizzaBox.Storing;
using PizzaBox.Storing.Repositories;

namespace PizzaBox.Client.Controllers
{
 
  [Route("/[controller]/[action]")]

  public class LoginController : Controller
  {
    private readonly PizzaBoxDbContext db;

    public LoginController(PizzaBoxDbContext dbContext){
      db = dbContext;
    }
    
    [Route("/")]
    public IActionResult Login(){

      return View("Login", new LoginViewModel());

    }

    public IActionResult Blank(){

      return View();
    }

    [HttpPost]
    public IActionResult gotoOrder(UserViewModel userViewModel){

      Console.WriteLine(userViewModel.User.Name);
      return View("Blank");

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Post(LoginViewModel loginViewModel){

      if (ModelState.IsValid){

        PizzaBoxRepository repo = new PizzaBoxRepository(); 

        UserViewModelClient userViewModelClient = repo.InitUser(loginViewModel.Username, loginViewModel.Password);

        UserViewModel userViewModel = new UserViewModel() {
          Orders = userViewModelClient.Orders,
          User = userViewModelClient.User,
        };

        TempData["username"] = userViewModel.User.Login.Username;
        TempData["password"] = userViewModel.User.Login.Password;
        TempData.Keep();

        System.Console.WriteLine("Login Controller: " + userViewModel.User.Name);

        return View("Index", userViewModel);
        
      }

      return View("Login", loginViewModel);

    }


  }

}