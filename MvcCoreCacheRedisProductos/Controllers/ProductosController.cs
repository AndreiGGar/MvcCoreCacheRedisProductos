using Microsoft.AspNetCore.Mvc;
using MvcCoreCacheRedisProductos.Models;
using MvcCoreCacheRedisProductos.Repositories;
using MvcCoreCacheRedisProductos.Services;

namespace MvcCoreCacheRedisProductos.Controllers
{
    public class ProductosController : Controller
    {
        private RepositoryProductos repo;
        private ServiceCacheRedis service;
        public ProductosController(RepositoryProductos repo, ServiceCacheRedis service)
        {
            this.repo = repo;
            this.service = service;
        }

        public IActionResult Index()
        {
            List<Producto> productos = this.repo.GetProductos();
            return View(productos);
        }

        public IActionResult Details(int id)
        {
            Producto producto = this.repo.FindProducto(id);
            return View(producto);
        }

        public async Task<IActionResult> Favoritos()
        {
            List<Producto> productosFavoritos = await this.service.GetProductosFavoritosAsync();
            return View(productosFavoritos);
        }

        public async Task<IActionResult> SeleccionarFavorito(int idproducto)
        {
            Producto producto = this.repo.FindProducto(idproducto);
            await this.service.AddProductoFavoritoAsync(producto);
            return RedirectToAction("Details", new { id = idproducto });
        }

        public async Task<IActionResult> DeleteFavorito(int idproducto)
        {
            await this.service.DeleteProductoFavoritoAsync(idproducto);
            return RedirectToAction("Favoritos");
        }
    }
}
