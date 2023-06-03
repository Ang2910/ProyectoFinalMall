using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalMall.Models;
using ProyectoFinalMall.ViewModels;
using ProyectoFinalMall.Views;

namespace ProyectoFinalMall.Catalogos
{
    public class CatalogoCliente 
    {

        TiendaropaContext context = new TiendaropaContext();

        public IEnumerable<Cliente> GetAllClientes()
        {
            return context.Cliente.OrderBy(x => x.Nombre);
        }
        public Cliente? GetIdUsuario(int id)
        {
            return context.Cliente.Find(id);
        }

        public void Recargar(Cliente c)
        {
            context.Entry(c).Reload();
        }

        public int spIniciarSesion(string correo, string password)
        {
            ////El parametro de correo y password es para indicar que parte de la cadena es el correo y que parte es el password.
            ////Asincrona = mucha carga de datos. Para que no se bloquee.
            ////Sincrona = IEnumerable.

            string cadena = $"select fnIniciarSesion('{correo}','{password}')";

            var y = ((IEnumerable<int>)context.Database.SqlQueryRaw<int>(cadena, correo, password).
                AsAsyncEnumerable<int>()).First();

            return y;
        }

        public void Create(Cliente c)
        {
            //contenedor.Add(u);
            context.Database.ExecuteSqlRaw($"call  sp_RegistrarCliente('{c.Nombre} ','{c.Correo}','{c.Contrasena}','{c.IdRol}')");
            context.SaveChanges();
        }

        public void Delete(Cliente c)
        {
            context.Cliente.Remove(c);
            context.SaveChanges();
        }
        public void Update(Cliente c)
        {
            context.Cliente.Update(c);
            context.SaveChanges();
        }

    }
}
