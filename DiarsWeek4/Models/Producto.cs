using System;
using System.Collections.Generic;

namespace DiarsWeek4.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Sku { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public int Stock { get; set; }

    public double Precio { get; set; }

    public string Estado { get; set; } = null!;
}
