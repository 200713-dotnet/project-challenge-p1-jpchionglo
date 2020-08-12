using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PizzaBox.Domain.Models;

namespace PizzaBox.Client.Models
{
    public class UserViewModelClient
    {

        public UserModel User {get;set;}

        public StoreModel Store {get;set;}

        public List<OrderModel> Orders {get;set;}

        public OrderModel Cart {get;set;}

        public UserViewModelClient(){

          Orders = new List<OrderModel>();

        }

    }
}
