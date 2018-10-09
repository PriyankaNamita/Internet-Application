using HealthCare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCare.Repository
{
    public class CustomerRepo
    {
        DatabaseContext dataBaseContext = new DatabaseContext();

        public Customer getByEmail(string email)
        {
            return dataBaseContext.Customers.FirstOrDefault(c => c.Email == email);
        }
    }
}