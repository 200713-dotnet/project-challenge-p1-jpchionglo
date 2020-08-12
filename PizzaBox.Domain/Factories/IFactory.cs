using PizzaBox.Domain.Models;

namespace PizzaBox.Domain.Factories
{
  public interface IFactory
  {

    AModel Create();

  }
}