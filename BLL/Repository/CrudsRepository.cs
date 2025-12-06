using BLL.Interface;
using DAL.Contexts;
using DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Repository
{
    public class CrudsRepository<T> : ICrudsRepository<T> where T : BaseEntity
    {
        private readonly CRUDSDbContext _context;

        public CrudsRepository(CRUDSDbContext context)
        {
            _context = context;
        }
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public T? Get(int id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        }
        public int Add(T model)
        {
            _context.Set<T>().Add(model);
            return _context.SaveChanges();
        }
        public int Update(T model)
        {
            _context.Set<T>().Update(model);
            return _context.SaveChanges();
        }
        public int Delete(T model)
        {
            _context.Set<T>().Remove(model);
            return _context.SaveChanges();
        }
        public decimal CalculateFinalPrice(decimal Price, decimal Tax, decimal Advertisement, decimal Discount)
        {
            decimal finalPrice = Price + Tax + Advertisement - Discount;
            return finalPrice;
        }
    }
}
