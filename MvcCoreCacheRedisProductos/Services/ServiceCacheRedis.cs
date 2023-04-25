using MvcCoreCacheRedisProductos.Helpers;
using MvcCoreCacheRedisProductos.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MvcCoreCacheRedisProductos.Services
{
    public class ServiceCacheRedis
    {
        private IDatabase database;

        public ServiceCacheRedis()
        {
            this.database = HelperCacheMultiplexer.GetConnection.GetDatabase();
        }

        public async Task AddProductoFavoritoAsync(Producto producto)
        {
            string jsonProductos = await this.database.StringGetAsync("favoritos");
            List<Producto> productosList;
            if (jsonProductos == null)
            {
                productosList = new List<Producto>();
            }
            else
            {
                productosList = JsonConvert.DeserializeObject<List<Producto>>(jsonProductos);
            }
            productosList.Add(producto);
            jsonProductos = JsonConvert.SerializeObject(productosList);
            this.database.StringSet("favoritos", jsonProductos);
        }

        public async Task<List<Producto>> GetProductosFavoritosAsync()
        {
            string jsonProductos = await this.database.StringGetAsync("favoritos");
            if (jsonProductos == null)
            {
                return null;
            }
            else
            {
                List<Producto> favoritos = JsonConvert.DeserializeObject<List<Producto>>(jsonProductos);
                return favoritos;
            }
        }

        public async Task DeleteProductoFavoritoAsync(int idproducto)
        {
            List<Producto> favoritos = await this.GetProductosFavoritosAsync();
            if (favoritos != null)
            {
                Producto productoDelete = favoritos.FirstOrDefault(z => z.IdProducto == idproducto);
                favoritos.Remove(productoDelete);
                if (favoritos.Count == 0)
                {
                    this.database.KeyDelete("favoritos");
                }
                else
                {
                    string jsonProductos = JsonConvert.SerializeObject(favoritos);
                    this.database.StringSet("favoritos", jsonProductos, TimeSpan.FromMinutes(30));
                }
            }
        }
    }
}
