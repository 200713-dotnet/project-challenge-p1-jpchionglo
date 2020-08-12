using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PizzaBox.Storing;
using PizzaBox.Domain.Models;
using PizzaBox.Client.Models;

namespace PizzaBox.Client.Controllers
{

  public class PizzaController : Controller
  {

    private readonly PizzaBoxDbContext _db;

    public PizzaController(PizzaBoxDbContext dbContext){

      _db = dbContext;

    }

    [Route("/[controller]/[action]")]
    public IActionResult Index(UserViewModel userViewModel){

      System.Console.WriteLine("Pizza Controller: " + userViewModel.User.Name);
      return View("index", userViewModel);

    }

    [Route("/[controller]/[action]")]
    [HttpGet()] // action filters (Authorization/Authentication, Action, Resource, Response, Exception)
    public IActionResult Get(){

      ViewBag.PizzaList = _db.Pizza.ToList();

      return View("Home", _db.Pizza.ToList());

    }


    [Route("/[controller]/[action]")]
    [HttpGet("{id}")]
    public IActionResult Show(int id){

      return View("Pizza", Get(id));

    }


    [HttpGet("{id}")]
    public PizzaModel Get(int id){

      return _db.Pizza.SingleOrDefault(p => p.Id == id);

    }

  }

}