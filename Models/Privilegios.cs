using System;
using System.Collections.Generic;

namespace ProyectoFinalMall.Models;

public partial class Privilegios
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Cliente> Cliente { get; set; } = new List<Cliente>();
}
