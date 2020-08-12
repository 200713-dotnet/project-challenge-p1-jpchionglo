using PizzaBox.Domain.Factories;

namespace PizzaBox.Domain.Models
{
  
  public class Brand
  {

    public IFactory PizzaFactory {get; private set;}

    //public IFactory OrderFactory {get; private set;}

    private static Brand brand;

    private Brand()
    {
      PizzaFactory = new PizzaFactory();
      //OrderFactory = new OrderFactory();
    }

    public static Brand Instance()
    {
      
      if (brand == null)
      {
        brand = new Brand();
      }

      return brand;

    }

  }

}