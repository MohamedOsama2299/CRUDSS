using AutoMapper;
using BLL.Interface.Services.Abstractions;
using DAL.Contexts;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using PL.DTOS;

namespace Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly CRUDSDbContext _context;
        public ProductRepository(CRUDSDbContext context )
        {
            _context = context;

        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateAsync(Product entity)
        {
            _context.Products.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Product?> UpdateAsync(int id, Product entity)
        {

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return null;
            product.Name = entity.Name;
            product.Price = entity.Price;
            product.CategoryID = entity.CategoryID;

            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Product>> SearchAsync(ProductSearchDTO search)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search.Name))
                query = query.Where(p => p.Name.Contains(search.Name));

            if (search.CategoryId.HasValue)
                query = query.Where(p => p.CategoryID == search.CategoryId.Value);

            if (search.MinPrice.HasValue)
                query = query.Where(p => p.Price >= search.MinPrice.Value);

            if (search.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= search.MaxPrice.Value);

            return await query.ToListAsync();
        }
        public async Task<decimal?> CalculateFinalPriceAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product == null)
                return null;
            decimal Total= product.Price + product.Tax + product.Advertisement - product.Discount;
            return Total;
        }

    }
}
