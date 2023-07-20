﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.BusinessLayer.Abstract
{
    public interface IGenericService<T>
    {
        Task<List<T>> GetListAll();
        Task<T> GetListByID(int id);
        Task<T> Create(T t);
        Task Delete(int id);
    }
}
