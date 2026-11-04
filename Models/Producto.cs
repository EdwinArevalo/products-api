using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Models;

public partial class Producto
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Range(0.01, 100000)]
    public decimal Precio { get; set; }

    [Range(0, 100000)]
    public int Stock { get; set; }

    public int CategoriaId { get; set; }

    public virtual Categoria? Categoria { get; set; }
}
