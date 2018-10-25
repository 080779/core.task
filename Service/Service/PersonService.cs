using Common;
using DTO;
using IService;
using Microsoft.EntityFrameworkCore;
using Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class PersonService : IPersonService
    {
        public long GetIdByName(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                //return dbc.Persons.Where(p => p.Name == name).Select(p => p.Id).SingleOrDefault();
                var person = dbc.Persons.SingleOrDefault();
                if(person==null)
                {
                    return 0;
                }
                return person.Id;
            }
        }
    }
}
