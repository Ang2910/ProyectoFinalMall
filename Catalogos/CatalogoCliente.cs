using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoFinalMall.Models;
using ProyectoFinalMall.ViewModels;
using ProyectoFinalMall.Views;
using System.Threading;
using System.Security.Principal;
using System.Text.RegularExpressions;

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

            if (y == 1)
            {
                var us = context.Cliente.Include(x => x.IdRolNavigation).FirstOrDefault(x => x.Correo == correo);
                if (us != null)
                    EstablecerTipoUsario(us);
            }
              
            return y;
        }

        public bool Validar(Cliente? u, out List<string> Errores)
        {
            Errores = new List<string>();

            if (u != null)
            {
                string patronNombre = @"[a-zA-z]";
                Regex regular = new Regex(patronNombre);

                string patronCorreo = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

                if (string.IsNullOrWhiteSpace(u.Nombre))
                {
                    Errores.Add("Ingrese el nombre del cliente.");
                }
                else if (regular.IsMatch(u.Nombre) == false)
                {
                    Errores.Add("El nombre del cliente debe estar en el formato correcto.");
                }
                if (string.IsNullOrWhiteSpace(u.Correo))
                {
                    Errores.Add("Ingrese el correo correspondiente.");
                }
                else if (regular.IsMatch(patronCorreo) == false)
                {
                    Errores.Add("Escriba el correo electronico en el formato correcto.");
                }
                if (string.IsNullOrWhiteSpace(u.Contrasena))
                {
                    Errores.Add("Escriba la contraseña.");
                } 
                if (context.Cliente.Any(x => x.Correo == u.Correo && x.Id != u.Id))
                {
                    Errores.Add("El correo electrónico ya se encuentra registrado.");
                }
            }

            return Errores.Count == 0;
        }


        public Cliente?GetCliente(string correo)
        {
            return context.Cliente.FirstOrDefault(x => x.Correo == correo);
        }

        public void Create(Cliente c)
        {
            //contenedor.Add(u);
            context.Database.ExecuteSqlRaw($"call sp_RegistrarCliente('{c.Nombre}','{c.Correo}','{c.Contrasena}', {c.IdRol})");
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
        private void EstablecerTipoUsario(Cliente c)
        {
            GenericIdentity user = new GenericIdentity(c.Nombre);
            if(c!= null)
            {

                string[] roles = new string[] { c.IdRolNavigation.Nombre };
                GenericPrincipal usprincipal = new GenericPrincipal(user, roles);
                Thread.CurrentPrincipal = usprincipal;
            }

        }

    }
}
