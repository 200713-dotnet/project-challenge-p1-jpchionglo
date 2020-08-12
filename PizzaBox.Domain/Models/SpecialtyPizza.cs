using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaBox.Domain.Models
{

  public class SpecialtyPizza
  {

    public int Id {get;set;}

    public PizzaModel Pizza {get;set;}

  }

}