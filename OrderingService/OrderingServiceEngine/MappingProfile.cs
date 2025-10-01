using AutoMapper;
using OrderingServiceData.Entities;
using OrderingServiceEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingServiceEngine
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add your mappings here
            CreateMap<Customer, CustomerModel>().ReverseMap();
            CreateMap<Item, ItemModel>().ReverseMap();
            CreateMap<Order, OrderModel>().ReverseMap();
            CreateMap<ApplicationLog, ApplicationLogModel>().ReverseMap();
        }
    }
}
