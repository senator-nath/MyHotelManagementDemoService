﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Contracts.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {

        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByColumnAsync(Expression<Func<T, bool>> predicate);
    }
}