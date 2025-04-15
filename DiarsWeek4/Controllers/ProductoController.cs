using DiarsWeek4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DiarsWeek4.Controllers
{
    public class ProductoController : Controller
    {
        #region Configuracion del controlador
        private readonly AppDbContext _context;
        public ProductoController(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        // Listar productos
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos.ToListAsync();
            return View("List", productos);
        }

        //Estados para las views
        private void CargarEstados(string estadoSeleccionado = null)
        {
            ViewBag.Estados = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "Activo", Value = "1" },
                new SelectListItem { Text = "Inactivo", Value = "0" }
            }, "Value", "Text", estadoSeleccionado);
        }


        #region Editar

        // GET: Producto/Edit/<id>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();

            CargarEstados(producto.Estado);
            return View(producto);
        }

        // POST: Producto/Edit/<id>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sku,Nombre,Stock,Precio,Estado")] Producto producto)
        {
            if (id != producto.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            CargarEstados(producto.Estado);

            return View(producto);
        }

        #endregion

        #region Registrar
        
        // GET: Producto/Create
        public IActionResult Create()
        {
            CargarEstados();
            return View();
        }

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Sku,Nombre,Stock,Precio,Estado")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            CargarEstados(producto.Estado);
            return View(producto);
            
        }
        
        #endregion

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }

    }
}
