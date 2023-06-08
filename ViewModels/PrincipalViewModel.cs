using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using ProyectoFinalMall.Catalogos;
using ProyectoFinalMall.Views;
using ProyectoFinalMall.Models;
using System.Security.RightsManagement;
using System.Security.Permissions;
using GalaSoft.MvvmLight.Command;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
 
namespace ProyectoFinalMall.ViewModels
{
    public class PrincipalViewModel : INotifyPropertyChanged

    {
        public Cliente? cliente { get; set;}

        public CatalogoCliente ccliente = new ();

        ClienteViewModel clientesviewmodel = new(); 
        public string Error { get; set; } = "";
        public string Modo { get; set;}
        public UserControl Vista { get; set;}

        LoginPrincipalView view;

        public ICommand CerrarSesionCommand { get; set; }
        public ICommand InicioSesionCommand { get; set; }
        public ICommand RegistrarCuentaCommand { get; set; }
        public ICommand VerRegistrarCommand { get; set; }
        public ICommand RegresarCommand { get; set; }

        public PrincipalViewModel()
        {
            VerRegistrarCommand = new RelayCommand(VerRegistrar);
            CerrarSesionCommand = new RelayCommand(CerrarLogin);
            InicioSesionCommand = new RelayCommand(InicioLogin);
            RegistrarCuentaCommand = new RelayCommand(Registrar);
            RegresarCommand = new RelayCommand(Regresar);
            cliente = new();
            view = new LoginPrincipalView()
            {
                DataContext = this    ///Referencia para ingresar los datos del usuario.
            };
            Vista = view;
            Actualizar();
        }

        private void CerrarLogin()
        {
            cliente = new();
            Vista = view;   //Se regresa a la ventana de principal. 
            Actualizar();
        }

        private void Regresar()
        {
            Modo = "login";
            Actualizar(); 
        }

        private void VerRegistrar()
        {
            cliente = new();
            Modo = "Registrar";
            Actualizar(); 
        }

        private void Registrar()
        {
            if (cliente != null)
            {
                if (ccliente.Validar(cliente, out List<string> Errores)) 
                {
                    //    if (usuario.Id != 0)
                    //    {
                    //        //Esta dentro de la coleccion
                    //        Error = "Este usuario ya se encuentra registrado.";
                    //    }
                    ccliente.Create(cliente);
                    clientesviewmodel.ActualizarBaseDatos();
                    Modo = "login";
                    Actualizar();
                }
                else
                {
                    foreach (var item in Errores)
                    {
                        Error = $"{Error}{item}{Environment.NewLine}";
                    }
                    Actualizar();
                }
                Error = "";
            }
        }

        private void InicioLogin()
        {
            if(cliente != null)
            {
                var inicio = ccliente.spIniciarSesion(cliente.Correo, cliente.Contrasena);
                if(inicio == 1)
                {
                    cliente = ccliente.GetCliente(cliente.Correo);
                    Actualizar();
                    if(Thread.CurrentPrincipal!= null)
                    {
                        if (Thread.CurrentPrincipal.IsInRole("Administrador"))
                            AccionesUsuarioAdministrador();
                        if (Thread.CurrentPrincipal.IsInRole("Cliente")) 
                            AccionesUsuarioCliente(); 
                    }
                }  
                else if(inicio == 3)
                 {
                    Error = "Contraseña Incorrecta.";
                    Actualizar();
                }             
                Actualizar();
            }
    }

        [Authorize(Roles = "Administrador")]
        private void AccionesUsuarioAdministrador()
        {
            Vista = new ClientesView() { }; 
        }
        [Authorize(Roles = "Cliente")]  
        private void AccionesUsuarioCliente() 
        {
            Vista = new MercanciaView(); 
        }
        void Actualizar(string? prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
