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

namespace ProyectoFinalMall.ViewModels
{
    public class PrincipalViewModel : INotifyPropertyChanged

    {
        public Cliente? cliente { get; set;}
        public CatalogoCliente ccliente = new ();

        public string Error { get; set; }
        public string Modo { get; set;}
        public UserControl Vista { get; set;}

        LoginPrincipalView view;

        public ICommand InicioSesionCommand { get; set; }

        public PrincipalViewModel()
        {

            InicioSesionCommand = new RelayCommand(InicioLogin);
            cliente = new();
            view = new LoginPrincipalView()
            {
                DataContext = this    ///Referencia para ingresar los datos del usuario.
            };
            Vista = view;
            Actualizar();
        }
        private void InicioLogin()
        {
            if(cliente != null)
            {
                var inicio = ccliente.spIniciarSesion(cliente.Correo, cliente.Contrasena);
                if(inicio == 1)
                {
                    Vista = new ClientesView();
                    Actualizar();
                }
                else if(inicio == 3)
                 {
                    Error = "Contraseña Incorrecta.";

                }
                Actualizar();
                Error = "";
            }
    }
        void Actualizar(string? prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
