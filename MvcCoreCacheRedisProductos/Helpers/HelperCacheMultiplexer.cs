using StackExchange.Redis;

namespace MvcCoreCacheRedisProductos.Helpers
{
    public static class HelperCacheMultiplexer
    {
        private static Lazy<ConnectionMultiplexer> CreateConnection =
            new Lazy<ConnectionMultiplexer>(() =>
            {
                string cnn = "bbddredis.redis.cache.windows.net:6380,password=QzuvJPU5Xv4978Bp3HMJKkMB7FUzAkMf6AzCaCDMqc8=,ssl=True,abortConnect=False";
                return ConnectionMultiplexer.Connect(cnn);
            });

        public static ConnectionMultiplexer GetConnection
        {
            get
            {
                return CreateConnection.Value;
            }
        }
    }
}
