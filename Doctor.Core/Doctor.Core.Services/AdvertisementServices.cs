using Doctor.Core.IRepository;
using Doctor.Core.IServices;
using Doctor.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Doctor.Core.Common.Attributes;

namespace Doctor.Core.Services
{
    public class AdvertisementServices : BaseServices<Advertisement>, IAdvertisementServices
    {
        public int Test()
        {
            return 1;
        }

        public IAdvertisementRepository dal;

        public AdvertisementServices(IAdvertisementRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }

        [Caching(AbsoluteExpiration = 10)]
        public List<AdvertisementEntity> TestAOP() => new List<AdvertisementEntity>() { new AdvertisementEntity() { id = 1, name = "laozhang" } };

        //public int Sum(int i, int j)
        //{
        //    return dal.Sum(i, j);

        //}

        //public int Add(Advertisement model)
        //{
        //    return dal.Add(model);
        //}

        //public bool Delete(Advertisement model)
        //{
        //    return dal.Delete(model);
        //}

        //public List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression)
        //{
        //    return dal.Query(whereExpression);

        //}

        //public bool Update(Advertisement model)
        //{
        //    return dal.Update(model);
        //}
    }
}
