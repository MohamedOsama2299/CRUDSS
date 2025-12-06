using BLL.Interface;
using BLL.Repository;
using DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using PL.Mapping;

namespace CRUDSS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<CRUDSDbContext>(options =>
                options.UseSqlite(connectionString));

            builder.Services.AddScoped(typeof(ICrudsRepository<>), typeof(CrudsRepository<>));
            builder.Services.AddAutoMapper(E => E.AddProfile(new CRUDSProfile()));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
