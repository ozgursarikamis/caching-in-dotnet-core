using System;
using EasyCaching.Core;
using Microsoft.AspNetCore.Mvc;

namespace RedisCaching.Web.Controllers
{
    [Route("/Redis")]
    public class RedisController : ControllerBase
    {
        private IEasyCachingProvider CachingProvider { get; }
        private IEasyCachingProviderFactory CachingProviderFactory { get; }

        public RedisController(
            IEasyCachingProviderFactory cachingProviderFactory
            )
        {
            CachingProviderFactory = cachingProviderFactory;
            CachingProvider = CachingProviderFactory.GetCachingProvider("redis1");
        }

        [HttpGet("Set")]
        public IActionResult SetItem()
        {
            CachingProvider.Set("cacheKey", "123", TimeSpan.FromSeconds(1));
            return Ok();
        }

        [HttpGet("Get")]
        public IActionResult GetItem()
        {
            var item = CachingProvider.Get<string>("cacheKey");
            return Ok(item);
        }
    }
}