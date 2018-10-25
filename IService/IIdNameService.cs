﻿using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface IIdNameService : IServiceSupport
    {
        Task<bool> DelByNameAsync(string name);
        Task<long> GetIdByNameAsync(string name);
        Task<IdNameDTO> GetByNameAsync(string name);
        Task<IdNameDTO[]> GetByTypeNameAsync(string typeName);
    }
}