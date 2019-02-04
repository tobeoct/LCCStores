using AutoMapper;
using LCCStores.DTO;
using LCCStores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCCStores.App_Start
{
    public class MappingProfile : Profile

        {
            public MappingProfile()
            {

                //            Map Model to Dto
                Mapper.CreateMap<Product, ProductDto>();
               

                // Map Dto to Model
                Mapper.CreateMap<ProductDto, Product>();
                

                //Map Model to ViewModel
               // Mapper.CreateMap<GLCategory, GLCategoryViewModel>();

                //Map ViewModel to Model
               // Mapper.CreateMap<GLCategoryViewModel, GLCategory>();


            }

        }
    }