using PizzaBox.Domain.Factories;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaBox.Domain.Models
{

  public class StoreModel : AModel
  {

    public LoginModel Login {get;set;}
    [NotMapped]
    private IFactory _factory;
    [NotMapped]
    private static StoreModel _store;
    private StoreModel(IFactory factory)
    {
      _factory = factory;
    }

    public StoreModel(){

      
    }

    public static StoreModel Instance(IFactory factory){

      if (_store == null || _store._factory.GetType().Name != factory.GetType().Name)
      {
        _store = new StoreModel(factory);
      }

      return _store;

    }

    public void CreateOrder()
    {
      AModel pm = _factory.Create();
    }


  }

}