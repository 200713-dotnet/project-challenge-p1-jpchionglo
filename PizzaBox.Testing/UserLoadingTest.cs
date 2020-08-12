using System;
using PizzaBox.Client.Models;
using PizzaBox.Domain.Models;
using PizzaBox.Storing;
using PizzaBox.Storing.Repositories;
using Xunit;

namespace PizzaBox.Testing
{
    public class UserLoadingTest
    {
        [Fact]
        public void Test1()
        {

          PizzaBoxRepository repo = new PizzaBoxRepository();

          UserViewModelClient User = new UserViewModelClient();
          User = repo.InitUser("jpchionglo","password");
          Console.WriteLine(User.User.Name);

        }
    }
}
