using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;

namespace Doctor.Core.AutoMapper
{
    /// <summary>
    /// 根据IMapperTo<>接口 自动初始化AutoMapper
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "AutoForIMapperTo";
            }
        }

        //protected override void Configure()
        //{
        //    base.Configure();

        //    typeof(SaveBuyerDemandRequest).Assembly.GetTypes()//SaveBuyerDemandRequest是TSource同属的Assembly底下的任意类，要包含多个Aeembly的话自己扩展咯
        //        .Where(i => i.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IMapperTo<>)))
        //        .ToList().ForEach(item =>
        //        {
        //            item.GetInterfaces()
        //                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IMapperTo<>))
        //                .ToList()//这里可以支持多个IMapperTo
        //                .ForEach(i => {
        //                    var t2 = i.GetGenericArguments()[0];
        //                    Mapper.CreateMap(item, t2);
        //                    Mapper.CreateMap(t2, item);
        //                });
        //        });
        //}
    }

    ////Class For Example
    //public class SaveBuyerDemandRequest : IMapperTo<BuyerDemandEntity>
    //{

    //}
}
