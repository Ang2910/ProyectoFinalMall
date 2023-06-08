using System;
using System.Collections.Generic;

namespace ProyectoFinalMall.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public int IdRol { get; set; }

    public virtual Privilegios IdRolNavigation { get; set; } = null!;
}
