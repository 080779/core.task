using Common;
using DTO;
using IService;
using Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ForwardStateService : IForwardStateService
    {
        public async Task<long> GetIdByNameAsync(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetIdAsync<ForwardStateEntity>(f=>f.Name==name);
            }
        }
    }
}
