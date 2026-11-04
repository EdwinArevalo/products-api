using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProductsAPI.Models;

public partial class Categoria
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string Nombre { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
