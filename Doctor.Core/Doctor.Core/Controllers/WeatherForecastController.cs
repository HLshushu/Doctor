using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Doctor.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Doctor.Core.IServices;

namespace Doctor.Core.Controllers
{
    /// <summary>
    /// Weather Forecast
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IAdvertisementServices advertisementServices;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IAdvertisementServices advertisementServices)
        {
            _logger = logger;
            this.advertisementServices = advertisementServices;
        }

        /// <summary>
        /// 获取接口数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string[] Get()
        {
            return Summaries;
        }

        /// <summary>
        /// 测试AOP
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<AdvertisementEntity> TestAdsFromAOP()
        {
            return advertisementServices.TestAOP();
        }

        /// <summary>
        /// Get data
        /// </summary>
        /// <returns></returns>
        // [HttpGet]
        // [Authorize(Roles = "Admin")]
        // [Authorize(Policy = "SystemOrAdmin")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        /// <summary>
        /// post
        /// </summary>
        /// <param name="love">model实体类参数</param>
        [HttpPost]
        public void Post(Love love)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "SystemOrAdmin")]
        public ActionResult<IEnumerable<string>> GetA()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
