using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Doctor.Core.IServices;
using Doctor.Core.Model;
using Doctor.Core.Common.Helper;
using Doctor.Core.Common.Redis;

namespace Doctor.Core.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize(Policy = "Admin")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IAdvertisementServices advertisementServices;
        private readonly IRedisCacheManager redisCacheManager;

        public DoctorController(IAdvertisementServices advertisementServices, IRedisCacheManager redisCacheManager)
        {
            this.advertisementServices = advertisementServices;
            this.redisCacheManager = redisCacheManager;
        }

        // GET: api/Doctor
        [HttpGet]
        public int Get(int i, int j)
        {
            // IAdvertisementServices advertisementServices = new AdvertisementServices();
            // return advertisementServices.Sum(i, j);
            return 5;
        }

        // GET: api/Doctor/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        public async Task<List<Advertisement>> Get(int id)
        {
            var connect = Appsettings.app(new string[] { "AppSettings", "RedisCaching", "ConnectionString" });//按照层级的顺序，依次写出来

            var advertisementList = new List<Advertisement>();

            if (redisCacheManager.Get<object>("Redis.Doctor") != null)
            {
                advertisementList = redisCacheManager.Get<List<Advertisement>>("Redis.Doctor");
            }
            else
            {
                advertisementList = await advertisementServices.Query(d => d.Id == id);
                redisCacheManager.Set("Redis.Doctor", advertisementList, TimeSpan.FromHours(2));
            }

            return advertisementList;
        }

        // POST: api/Doctor
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Doctor/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
