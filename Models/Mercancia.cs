using System;
using System.Collections.Generic;

namespace ProyectoFinalMall.Models;

public partial class Mercancia
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Tipo { get; set; }

    public string? Nacionalidad { get; set; }

    public string? Marca { get; set; }

    public decimal? Precio { get; set; }

    public string? Descripcion { get; set; }
}
