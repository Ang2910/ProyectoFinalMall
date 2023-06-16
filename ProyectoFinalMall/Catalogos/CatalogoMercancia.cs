using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoFinalMall.Views;
using ProyectoFinalMall.ViewModels;
using ProyectoFinalMall.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ProyectoFinalMall.Catalogos
{
    public class CatalogoMercancia
    {

        TiendaropaContext context = new TiendaropaContext();

        public IEnumerable<Mercancia> GetAllMercancia()
        {
            return context.Mercancia.OrderBy(x => x.Nombre);
        }
        public Mercancia? GetIdMercancia(int id)
        {
            return context.Mercancia.Find(id);
        }

        public void Recargar(Mercancia m)
        {
            context.Entry(m).Reload();
        }
        public void Create(Mercancia m)
        {
            //contenedor.Add(u);
            context.Mercancia.Add(m);
            context.SaveChanges();
        }

        public void Delete(Mercancia m)
        {
            context.Mercancia.Remove(m);
            context.SaveChanges();
        }
        public void Update(Mercancia m)
        {
            context.Mercancia.Update(m);
            context.SaveChanges();
        }
        public bool Validar(Mercancia? m, out List<string> Errores)
        {
            Errores = new List<string>();

            if (m != null)
            {
                if (string.IsNullOrWhiteSpace(m.Nombre))
                {
                    Errores.Add("Ingrese el nombre de la mercancia.");
                }
                if (!Uri.TryCreate(m.Imagen, UriKind.Absolute, out var uri))//Validan si una cadena de texto m.Imagen es una URL de imagen válida utilizando el método TryCreate() de la clase Uri.
                {
                    Errores.Add("Ingrese una URL de la imagen valida");
                }
                if (string.IsNullOrWhiteSpace(m.Tipo))
                {
                    Errores.Add("Escriba el tipo de la mercancia.");
                }
                if (string.IsNullOrWhiteSpace(m.Nacionalidad))
                {
                    Errores.Add("Ingrese la nacionalidad correspondiente.");
                }
                if (string.IsNullOrWhiteSpace(m.Marca))
                {
                    Errores.Add("El campo marca no puede estar vacio.");
                }
                if (m.Precio == null || string.IsNullOrWhiteSpace(m.Precio.ToString()))
                {
                    Errores.Add("El precio no puede estar vacío.");
                }
                else
                {
                    decimal precio;
                    if (!decimal.TryParse(m.Precio.ToString(), out precio))//Cnvertir una cadena de texto en formato de precio a un valor decimal. 
                    {
                        Errores.Add("El precio no es válido.");
                    }

                }
                if (string.IsNullOrWhiteSpace(m.Descripcion))
                {
                    Errores.Add("La descripcion no puede estar vacia.");
                }
            }
            return Errores.Count == 0;
        }
    }
}

