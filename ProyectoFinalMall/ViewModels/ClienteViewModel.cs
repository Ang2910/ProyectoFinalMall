using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoFinalMall.Views;
using ProyectoFinalMall.ViewModels;
using System.Security.RightsManagement;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using ProyectoFinalMall.Models;
using System.Windows.Input;
using ProyectoFinalMall.Catalogos;
using GalaSoft.MvvmLight.Command;
using System.Windows.Media;
 
namespace ProyectoFinalMall.ViewModels
{
    public class ClienteViewModel : INotifyPropertyChanged
    {

        public UserControl Vista { get; set; }

        public ObservableCollection<Cliente> ListaClientes { get; set; } = new ObservableCollection<Cliente>();

        public ObservableCollection<Vistamercanciacliente> ListaVista { get; set; } = new ObservableCollection<Vistamercanciacliente>();

        public Cliente? cliente { get; set; }

        public string? Error { get; set; }

        public CatalogoCliente catalago = new();
        public string Modo { get; set; }

        private string filtroNombre;

        public string FiltroNombre
        {
            get { return filtroNombre; } 
            set 
            { 
            
                filtroNombre = value;
                FiltrarClientes();
                Actualizar(nameof(FiltroNombre));

            }
        }
       

        public ICommand AgregarClienteCommand { get; set; }
        public ICommand EliminarClienteCommand { get; set; }
        public ICommand AceptarClienteCommand { get; set; }
        public ICommand EditarClienteCommand { get; set; }
        public ICommand GuardarClienteCommand { get; set; }
        public ICommand CancelarCommand { get; set; } 
        public ICommand RegresarCommand { get; set; } 
        public ICommand VerDatosCommand { get; set; }
        public ICommand VerClientesMercanciaCommand { get; set; }
  

        public ClienteViewModel()
        {
            var Cliente = catalago.GetAllVistaMercanciaCliente();
            foreach (var cliente in Cliente)
            {
                ListaVista.Add(cliente);
            }

            AgregarClienteCommand = new RelayCommand(AgregarCliente);
            EliminarClienteCommand = new RelayCommand<Cliente>(EliminarCliente);
            AceptarClienteCommand = new RelayCommand(Aceptar);
            EditarClienteCommand = new RelayCommand<Cliente>(EditarCliente);
            GuardarClienteCommand = new RelayCommand(GuardarCliente);
            CancelarCommand = new RelayCommand(CancelarCliente);
            RegresarCommand = new RelayCommand(Regresar);
            VerDatosCommand = new RelayCommand<int>(VerDatos);
            VerClientesMercanciaCommand = new RelayCommand(VerCliente);
            Modo = "Ver"; 
            ActualizarBaseDatos();
            Actualizar();
        } 
        private void VerCliente()
        {
            Modo = "verClientesMercanciaView";
            Actualizar();
        } 
        private void FiltrarClientes()
        {
            if(string.IsNullOrEmpty(FiltroNombre))
            {
                ActualizarBaseDatos();
            }
            else 
            {

                ListaClientes.Clear();

                foreach(var cliente in catalago.GetAllClientes()) 
                { 
                
                    if(cliente.Nombre.Contains(FiltroNombre,StringComparison.OrdinalIgnoreCase))
                    {

                        ListaClientes.Add(cliente);

                    }
                
                }
                Actualizar();
            }
        }

        private void VerDatos(int id)
        {
            cliente = catalago.GetIdUsuario(id);
            Modo = "VerDatos";
            Actualizar();
        }

        private void Regresar()
        {
            Modo = "Ver";
            Actualizar();
        }

        private void CancelarCliente()
        {
            if (cliente != null)
            {
                catalago.Recargar(cliente);
                Modo = "Ver";
                Actualizar();
            }
        }
        Random r = new Random(); 
        private void GuardarCliente()
        {  
            if (cliente != null)
            {
                Error = "";

                if (catalago.Validar(cliente, out List<string> errores))
                {
                    if (cliente.Id != 0)
                    {
                        //Esta dentro de la coleccion

                        catalago.Update(cliente);
                    }
                    else
                    { 
                        //cliente.IdRol = r.Next(1, 2);
                        catalago.Create(cliente);
                    }
        
                    ActualizarBaseDatos();
                    Modo = "Ver";
                    Actualizar(); 
                }
                else
                {
                    foreach (var i in errores)
                    {
                        Error = $"{Error}{i}{Environment.NewLine}";
                    }
                    Actualizar();


                }

            }
        }   
        private void EditarCliente(Cliente obj)
        {
            if(obj != null)
                    {
                        Error = "";
                        cliente = obj;
                        Modo = "Editar";
                        Actualizar();

                    }
        }

        private void Aceptar()
        {
                    if(cliente != null)
                    {
                        catalago.Delete(cliente);
                        ActualizarBaseDatos();
                        Modo = "Ver";
                        Actualizar();
                    }
            
        }

        private void EliminarCliente(Cliente obj)
        {
                    if(obj != null)
                    {
                        Error = "";
                        cliente = obj;
                        Modo = "Eliminar";
                        Actualizar();


                    }
            
        } 
        private void AgregarCliente()
        {
                    cliente = new();
                    Modo = "Agregar";
                    Error = "";
                    Actualizar();
        }
       


        public void ActualizarBaseDatos()
        {
           ListaClientes.Clear();

            foreach (var i in catalago.GetAllClientes())
            {
                ListaClientes.Add(i);
            }
            Actualizar();

        }
       

        void Actualizar(string? propiedad = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propiedad));
        }
       

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
