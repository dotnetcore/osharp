using Microsoft.Extensions.Caching.Distributed;

namespace Liuliu.Demo.Web.Startups.Yitter
{
    public class DistributedLock : IDisposable
    {
        private readonly IDistributedCache _cache;
        private readonly string _lockKey;
        private bool _hasLock;

        public DistributedLock(IDistributedCache cache, string lockKey)
        {
            _cache = cache;
            _lockKey = lockKey;
        }

        public async Task<bool> AcquireAsync(TimeSpan timeout)
        {
            var expiration = DateTimeOffset.UtcNow.Add(timeout);
            var lockValue = Guid.NewGuid().ToString();

            while (DateTimeOffset.UtcNow < expiration)
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                };

                if (await _cache.GetStringAsync(_lockKey) == null)
                {
                    await _cache.SetStringAsync(_lockKey, lockValue, options);
                    _hasLock = true;
                    return true;
                }

                await Task.Delay(100);
            }

            return false;
        }

        public void Release()
        {
            if (_hasLock)
            {
                _cache.Remove(_lockKey);
                _hasLock = false;
            }
        }

        public void Dispose()
        {
            Release();
        }
    }

}
