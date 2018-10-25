using Common;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Entity;
using System;
using System.Linq;

namespace Test
{
    class Program
    {
        public class Test
        {
            public void Get(int i)
            {

            }
            public void Get(long i)
            {

            }
        }
        static void Main(string[] args)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                //AdminEntity admin = new AdminEntity();
                //admin.Name = "admin";
                //admin.Mobile = "15615615616";
                //admin.Description = "系统管理员";
                //admin.Salt = CommonHelper.GetCaptcha(4);
                //admin.Password = "1" + admin.Salt;
                PersonEntity person = new PersonEntity();
                person.Name = "周2";
                dbc.Persons.Add(person);
                dbc.SaveChanges();
                long id = person.Id;
                Console.WriteLine(id);
                Console.ReadKey();
            }
        }
    }
}
