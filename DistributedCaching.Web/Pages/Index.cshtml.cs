using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace DistributedCaching.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IDistributedCache DistributedCache { get; }
        public string CachedTimeUtc { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IDistributedCache distributedCache)
        {
            _logger = logger;
            DistributedCache = distributedCache;
        }

        public async Task OnGetAsync()
        {
            CachedTimeUtc = "Cached Time Expired";
            var encodedCachedTime = await DistributedCache.GetAsync("cachedTimeUTC");

            if (encodedCachedTime != null)
            {
                CachedTimeUtc = Encoding.UTF8.GetString(encodedCachedTime);
            }
        }

        public async Task<IActionResult> OnPostResetCachedTime()
        {
            var currentTimeUtc = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            var encodedTimeUtc = Encoding.UTF8.GetBytes(currentTimeUtc);

            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(20));

            await DistributedCache.SetAsync("cachedTimeUTC", encodedTimeUtc, options);

            return RedirectToPage();
        }
    }
}