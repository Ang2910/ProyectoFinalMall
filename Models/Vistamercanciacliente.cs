using System;
using System.Collections.Generic;

namespace ProyectoFinalMall.Models;

public partial class Vistamercanciacliente
{
    public int Id { get; set; }

    public string? NombreMercancia { get; set; }

    public string? Tipo { get; set; }

    public string? Nacionalidad { get; set; }

    public string? Marca { get; set; }

    public decimal? Precio { get; set; }

    public string? Descripcion { get; set; }

    public string NombreCliente { get; set; } = null!;

    public string Correo { get; set; } = null!;
}
