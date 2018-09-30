using Service;
using Service.Entity;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity user = new UserEntity();
                user.Name = "aaa";
                dbc.Users.Add(user);
                dbc.SaveChanges();
                Console.WriteLine("ok");
                Console.ReadKey();
            }
        }
    }
}
