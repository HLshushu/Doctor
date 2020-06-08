using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Doctor.Core.IServices;
using Doctor.Core.Model;

namespace Doctor.Core.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize(Policy = "Admin")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IAdvertisementServices advertisementServices;

        public DoctorController(IAdvertisementServices advertisementServices)
        {
            this.advertisementServices = advertisementServices;
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
            return await advertisementServices.Query(d => d.Id == id);
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
