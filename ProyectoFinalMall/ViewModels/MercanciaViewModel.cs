using GalaSoft.MvvmLight.Command;
using ProyectoFinalMall.Catalogos;
using ProyectoFinalMall.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProyectoFinalMall.Views;
using System.Security.Cryptography.Xml;
using Newtonsoft.Json;
using System.IO;

namespace ProyectoFinalMall.ViewModels
{
    public class MercanciaViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Mercancia> ListaMercancia { get; set; } = new ObservableCollection<Mercancia>();

        public CatalogoMercancia cmercancia = new();

        private Mercancia _mercancia;

        public Mercancia mercancia
        {
            get { return _mercancia; }
            set { _mercancia = value; PropertyChange(); }
        }

        public string Vista { get; set; } = "Ver";

        public string? Error { get; set; }

        public ICommand CancelarCommand { get; set; }

        public ICommand RegresarCommand { get; set; }

        public ICommand AgregarMercanciaCommand { get; set; }

        public ICommand AceptarMercanciaCommand { get; set; }

        public ICommand EditarMercanciaCommand { get; set; }

        public ICommand GuardarMercanciaCommand { get; set; }

        public ICommand EliminarMercanciaCommand { get; set; }


        public MercanciaViewModel()
        {

           
            CancelarCommand = new RelayCommand(Cancelar);
            RegresarCommand = new RelayCommand(Regresar);
            AgregarMercanciaCommand = new RelayCommand(Agregar);
            AceptarMercanciaCommand = new RelayCommand(Aceptar);
            EditarMercanciaCommand = new RelayCommand<Mercancia>(Editar);
            GuardarMercanciaCommand = new RelayCommand(Guardar);
            EliminarMercanciaCommand = new RelayCommand<Mercancia>(Eliminar);
            Vista = "Ver";
            ActualizarBaseDatos();
            PropertyChange();

        }

       

        private void Aceptar()
        {
            if (mercancia != null)
            {
                cmercancia.Delete(mercancia);
                ActualizarBaseDatos();
                Vista = "Ver";
                PropertyChange(); 
            }

        }

        private void Editar(Mercancia obj)
        { 
            if (obj != null)
            {
                Error = "";
                mercancia = obj;
                Vista = "Editar";
                PropertyChange(); 

            }

        }
        private void Regresar()
        {
            Vista = "Ver";
            PropertyChange();

        }
        private void Eliminar(Mercancia obj)
        {
            if (obj != null)
            {
                Error = "";
                mercancia = obj;
                Vista = "Eliminar";
                PropertyChange(); 
            }
        }
        Random r = new Random();
        private void Guardar()
        {
            if (mercancia != null)
            {    
                Error = "";

                if (cmercancia.Validar(mercancia, out List<string> errores))
                {
                    if (mercancia.Id != 0)
                    {
                        //Esta dentro de la coleccion
                        
                        cmercancia.Update(mercancia);
                        
                    }
                    else
                    {
                        cmercancia.Create(mercancia);
                        ActualizarBaseDatos();
                        Vista = "Ver";
                        PropertyChange();
                    }    
                }
                else
                {
                    foreach (var i in errores)
                    {
                        Error = $"{Error}{i}{Environment.NewLine}";
                    }
                    PropertyChange();
                }
            }
        } 
        private void Agregar()
        {
            mercancia = new();
            Vista = "Editar";
            Error = "";
            PropertyChange();

        }
        private void Cancelar()
        {

            if (mercancia != null)
            {
                cmercancia.Recargar(mercancia);
                Vista = "Ver";
                PropertyChange(); 
            }

        }
        public void ActualizarBaseDatos()
        {
            ListaMercancia.Clear();

            foreach (var i in cmercancia.GetAllMercancia())
            {
                ListaMercancia.Add(i);
            }
            PropertyChange(); 

        }

        void PropertyChange(string? propiedad = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propiedad));
        }



        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

