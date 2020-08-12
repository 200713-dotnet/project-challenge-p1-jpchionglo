using System;
using System.ComponentModel.DataAnnotations;
using PizzaBox.Domain.Models;

namespace PizzaBox.Client.Models
{
    public class LoginViewModel
    {

        [Required]
        public string Username {get;set;}

        [Required]
        public string Password {get;set;}

    }
}
