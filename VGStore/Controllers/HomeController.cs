using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VGStore.Data;
using VGStore.Models;
using VGStore.Models.ViewModels;
using VGStore.Utility;

namespace VGStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;
        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Productos = _db.Productos.Include(u => u.Categoria).Include(u => u.Consoles),
                Consolas = _db.Consoles,
                Categories = _db.Categories
            };
            return View(homeVM);
        }

        public IActionResult Detalles(int id)
        {
            List<CarritoCompras> ListaCarrito = new List<CarritoCompras>();
            if (HttpContext.Session.Get<IEnumerable<CarritoCompras>>(WC.SessionCarrito) != null &&
                HttpContext.Session.Get<IEnumerable<CarritoCompras>>(WC.SessionCarrito).Count() > 0)
            {
                ListaCarrito = HttpContext.Session.Get<List<CarritoCompras>>(WC.SessionCarrito);
            }
            DetallesVM DetallesVM = new DetallesVM()
            {
                Producto = _db.Productos.Include(u => u.Categoria).Include(u => u.Consoles).Where(u => u.IdProducto == id).FirstOrDefault(),
                EnCarrito = false
            };
            foreach(var item in ListaCarrito)
            {
                if(item.ProductoId == id)
                {
                    DetallesVM.EnCarrito = true;
                }
            }
            return View(DetallesVM);
        }
        [HttpPost, ActionName("Detalles")]
        public IActionResult DetallesPost(int id)
        {
            List<CarritoCompras> ListaCarrito = new List<CarritoCompras>();
            if(HttpContext.Session.Get<IEnumerable<CarritoCompras>>(WC.SessionCarrito) != null &&
                HttpContext.Session.Get<IEnumerable<CarritoCompras>>(WC.SessionCarrito).Count() > 0)
            {
                ListaCarrito = HttpContext.Session.Get<List<CarritoCompras>>(WC.SessionCarrito);
            }
            ListaCarrito.Add(new CarritoCompras { ProductoId = id });
            HttpContext.Session.Set(WC.SessionCarrito, ListaCarrito);
            return RedirectToAction(nameof(Index));
        }

         public IActionResult RemoverCarrito(int id)
        {
            List<CarritoCompras> ListaCarrito = new List<CarritoCompras>();
            if (HttpContext.Session.Get<IEnumerable<CarritoCompras>>(WC.SessionCarrito) != null &&
                HttpContext.Session.Get<IEnumerable<CarritoCompras>>(WC.SessionCarrito).Count() > 0)
            {
                ListaCarrito = HttpContext.Session.Get<List<CarritoCompras>>(WC.SessionCarrito);
            }

            var itemRemover = ListaCarrito.SingleOrDefault(r => r.ProductoId == id);
            if(itemRemover != null)
            {
                ListaCarrito.Remove(itemRemover);
            }
            HttpContext.Session.Set(WC.SessionCarrito, ListaCarrito);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
