﻿using Doctor.Core.IRepository;
using Doctor.Core.IServices;
using Doctor.Core.Model;
using Doctor.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Doctor.Core.Services
{
    public class AdvertisementServices : BaseServices<Advertisement>, IAdvertisementServices
    {
        //public IAdvertisementRepository dal = new AdvertisementRepository();

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
