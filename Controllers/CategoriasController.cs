using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ProductosContext _context;

    public CategoriasController(ProductosContext context) => _context = context;

    // GET api/categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> Get()
    {
        var categorias = await _context.Categorias
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
            .ToListAsync();
        return Ok(categorias);
    }

    // GET api/categorias/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Categoria>> Get(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return NotFound();
        return Ok(categoria);
    }

    // GET api/categorias/1/productos  (ruta anidada)
    [HttpGet("{id}/productos")]
    public async Task<ActionResult<IEnumerable<Producto>>> GetProductos(int id)
    {
        if (!await _context.Categorias.AnyAsync(c => c.Id == id)) return NotFound();

        var productos = await _context.Productos
            .AsNoTracking()
            .Where(p => p.CategoriaId == id)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
        return Ok(productos);
    }

    // POST api/categorias
    [HttpPost]
    public async Task<ActionResult<Categoria>> Post(Categoria categoria)
    {
        if (await _context.Categorias.AnyAsync(c => c.Nombre == categoria.Nombre))
            return Conflict(new ProblemDetails { Title = "Categoría duplicada", Detail = $"Ya existe la categoría '{categoria.Nombre}'.", Status = StatusCodes.Status409Conflict });

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = categoria.Id }, categoria);
    }

    // PUT api/categorias/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Categoria categoria)
    {
        if (id != categoria.Id) return BadRequest();

        var actual = await _context.Categorias.FindAsync(id);
        if (actual == null) return NotFound();

        if (await _context.Categorias.AnyAsync(c => c.Nombre == categoria.Nombre && c.Id != id))
            return Conflict(new ProblemDetails { Title = "Categoría duplicada", Detail = $"Ya existe la categoría '{categoria.Nombre}'.", Status = StatusCodes.Status409Conflict });

        actual.Nombre = categoria.Nombre;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE api/categorias/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return NotFound();

        if (await _context.Productos.AnyAsync(p => p.CategoriaId == id))
            return Conflict(new ProblemDetails { Title = "Categoría con productos", Detail = "No se puede eliminar una categoría que tiene productos.", Status = StatusCodes.Status409Conflict });

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
