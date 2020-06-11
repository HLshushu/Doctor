using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor.Core.Model;
using AutoMapper;

namespace Doctor.Core.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            CreateMap<DoctorArticle, DoctorArticleViewModels>();
            CreateMap<DoctorArticleViewModels, DoctorArticle>();

        }
    }
}
