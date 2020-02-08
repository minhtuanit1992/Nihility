using AutoMapper;
using Nihility.Model.Models;
using Nihility.X0.Solution.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nihility.X0.Solution.Mappings
{
    /// <summary>
    /// - Lớp cấu hình để tự động Mapping DataModel vào ViewModel
    /// - Nuget Packet: AutoMapper
    /// </summary>
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Post, PostViewModel>();
            });

            IMapper iMapper = config.CreateMapper();


        }
    }
}