using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PizzaBox.Domain.Models;

namespace PizzaBox.Client.Models
{
    public class CartViewModel
    {

        public UserModel User {get;set;}

        public StoreModel Store {get;set;}

        public List<OrderModel> Orders {get;set;}

        public OrderModel Cart {get;set;}



        public List<string> Crusts { get; set; }
        public List<string> Sizes { get; set; }
        public List<string> Toppings { get; set; }
        public List<string> Stores {get;set;}

        public List<string> SpecialtyPizzas {get;set;}

        public List<CheckBoxTopping> Toppings2 {get;set;}

        public CartViewModel(){

          Orders = new List<OrderModel>();
          Crusts = new List<string>();
          Sizes = new List<string>();
          Toppings = new List<string>();
          Stores = new List<string>();
          SelectedToppings2 = new List<CheckBoxTopping>();
          Toppings2 = new List<CheckBoxTopping>();
          Cart = new OrderModel();

        }

        public void AddToppingToList(string topping){

          Toppings2.Add(new CheckBoxTopping(){Text = topping});

        }

        [Required]
        public string Crust {get;set;}
        [Required]
        public string Size {get;set;}

        public string PizzaNumber {get;set;}

        public string StoreString {get;set;}

        public string SpecialtyPizza {get;set;}
        //[Range(2,5)]
        public List<string> SelectedToppings {get;set;}
        public List<CheckBoxTopping> SelectedToppings2 {get;set;}
        public string SelectedToppings3 {get;set;}

        public void AddSelectedTopping(string top) {

          SelectedToppings2.Add(new CheckBoxTopping() {Text = top});

        }

        public void ClearSelectedToppings(){

          SelectedToppings2.Clear();

        }

        public class CheckBoxTopping : ToppingModel
        {
          public string Text {get;set;}
          public bool isSelected {get;set;}
        }

    }
}
