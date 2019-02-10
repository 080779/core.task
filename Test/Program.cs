using Common;
using Common.Enums;
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
        static void Main1(string[] args)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                //AdminEntity admin = new AdminEntity();
                //admin.Name = "admin";
                //admin.Mobile = "15615615616";
                //admin.Description = "系统管理员";
                //admin.Salt = CommonHelper.GetCaptcha(4);
                //admin.Password = "1" + admin.Salt;
                Console.ReadKey();
            }
        }
        static void Main(string[] args)
        {
            int[,] values = new int[2, 2];
            Console.ReadKey();
        }
        public class Task1
        {
            public long Id { get; set; }
            public int IsEnabled { get; set; }
        }
    }
}
