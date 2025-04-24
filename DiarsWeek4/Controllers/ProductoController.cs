using DiarsWeek4.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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


        #region Crear o Editar

        // GET: Producto/CreateEdit/<id> (unificado para Crear y Editar)
        // Mostrar el formulario
        public async Task<IActionResult> CreateEdit(int? id)
        {
            Producto producto = new Producto();

            if(id.HasValue) // Si existe el id, editamos
            {
                producto = await _context.Productos.FindAsync(id);
                if (producto == null)
                    return NotFound();
            }

            CargarEstados(producto.Estado);
            return View(producto);
        }

        // POST: Producto/CreateEdit/<id>
        // Procesar el formulario - Crear o editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEdit(int? id, [Bind("Id,Sku,Nombre,Stock,Precio,Estado")] Producto producto)
        {
            if (id.HasValue && id != producto.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                if (producto.Id == 0) // Si el id es 0 es nuevo producto
                {
                    _context.Add(producto);
                }
                else
                {
                    _context.Update(producto);

                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                    
            }

            CargarEstados(producto.Estado);
            return View(producto);
        }
        #endregion

        
        #region Eliminar

        // Eliminar producto por id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

    }
}
