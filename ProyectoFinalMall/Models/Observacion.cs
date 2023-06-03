using System;
using System.Collections.Generic;

namespace ProyectoFinalMall.Models;

public partial class Observacion
{
    public int Id { get; set; }

    public string Correo { get; set; } = null!;

    public string Bitacora { get; set; } = null!;

    public DateTime? Fecha { get; set; }
}
