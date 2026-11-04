using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly ProductosContext _context;

    public ProductosController(ProductosContext context) => _context = context;

    // GET api/productos?categoriaId=1&buscar=mouse
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Producto>>> Get(int? categoriaId, string? buscar)
    {
        var consulta = _context.Productos.Include(p => p.Categoria).AsNoTracking();

        if (categoriaId != null)
            consulta = consulta.Where(p => p.CategoriaId == categoriaId);
        if (!string.IsNullOrWhiteSpace(buscar))
            consulta = consulta.Where(p => p.Nombre.Contains(buscar));

        var productos = await consulta.OrderBy(p => p.Nombre).ToListAsync();
        return Ok(productos);
    }

    // GET api/productos/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Producto>> Get(int id)
    {
        var producto = await _context.Productos
            .Include(p => p.Categoria)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
        if (producto == null) return NotFound();
        return Ok(producto);
    }

    // POST api/productos
    [HttpPost]
    public async Task<ActionResult<Producto>> Post(Producto producto)
    {
        if (!await _context.Categorias.AnyAsync(c => c.Id == producto.CategoriaId))
        {
            ModelState.AddModelError(nameof(producto.CategoriaId), "La categoría indicada no existe.");
            return ValidationProblem(ModelState);
        }

        producto.Categoria = null;
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = producto.Id }, producto);
    }

    // PUT api/productos/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Producto producto)
    {
        if (id != producto.Id) return BadRequest();

        var actual = await _context.Productos.FindAsync(id);
        if (actual == null) return NotFound();

        if (!await _context.Categorias.AnyAsync(c => c.Id == producto.CategoriaId))
        {
            ModelState.AddModelError(nameof(producto.CategoriaId), "La categoría indicada no existe.");
            return ValidationProblem(ModelState);
        }

        actual.Nombre = producto.Nombre;
        actual.Precio = producto.Precio;
        actual.Stock = producto.Stock;
        actual.CategoriaId = producto.CategoriaId;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE api/productos/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null) return NotFound();

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
