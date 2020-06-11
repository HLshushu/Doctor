using Doctor.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Doctor.Core.IServices
{
    public interface IAdvertisementServices : IBaseServices<Advertisement>
    {
        //int Sum(int i, int j);
        //int Add(Advertisement model);
        //bool Delete(Advertisement model);
        //bool Update(Advertisement model);
        //List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression);
        List<AdvertisementEntity> TestAOP();
        Task<List<DoctorArticle>> getDcotors();
        Task<DoctorArticleViewModels> getDoctorDetails(int id);
    }
}
