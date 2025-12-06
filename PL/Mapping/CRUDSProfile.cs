using AutoMapper;
using DAL.Models;
using PL.DTOS;

namespace PL.Mapping
{
    public class CRUDSProfile : Profile
    {
        public CRUDSProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}