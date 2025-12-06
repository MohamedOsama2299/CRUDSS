using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interface
{
    public interface ICrudsRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T? Get(int id);
        int Add(T model);
        int Update(T model);
        int Delete(T model);
        decimal CalculateFinalPrice(decimal Price, decimal Tax, decimal Advertisement, decimal Discount);
    }
}
