using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using VGStore.Data;
using VGStore.Models;
using VGStore.Utility;

namespace VGStore.Controllers
{
    [Authorize]
    public class CarritoController : Controller
    {
        public readonly AppDbContext _db;

        public CarritoController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<CarritoCompras> ListaCarrito = new List<CarritoCompras>();
            if(HttpContext.Session.Get<IEnumerable<CarritoCompras>>(WC.SessionCarrito) != null &&
                HttpContext.Session.Get<IEnumerable<CarritoCompras>>(WC.SessionCarrito).Count() > 0)
            {
                ListaCarrito = HttpContext.Session.Get<List<CarritoCompras>>(WC.SessionCarrito);
            }
            List<int> productoEnCarrito = ListaCarrito.Select(i => i.ProductoId).ToList();
            IEnumerable<Productos> prodList = _db.Productos.Where(u => productoEnCarrito.Contains(u.IdProducto)).Include(u => u.Categoria).Include(u => u.Consoles);
         
            return View(prodList);
        }
        public IActionResult Remover(int id)
        {
            List<CarritoCompras> ListaCarrito = new List<CarritoCompras>();
            if (HttpContext.Session.Get<IEnumerable<CarritoCompras>>(WC.SessionCarrito) != null &&
              HttpContext.Session.Get<IEnumerable<CarritoCompras>>(WC.SessionCarrito).Count() > 0)
            {
                ListaCarrito = HttpContext.Session.Get<List<CarritoCompras>>(WC.SessionCarrito);
            }
            ListaCarrito.Remove(ListaCarrito.FirstOrDefault(u => u.ProductoId == id));
            HttpContext.Session.Set(WC.SessionCarrito, ListaCarrito);

            return RedirectToAction(nameof(Index));
        }
    }
}
