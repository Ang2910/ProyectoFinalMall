using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoFinalMall.Views
{
    /// <summary>
    /// Lógica de interacción para LoginPrincipalView.xaml
    /// </summary>
    public partial class LoginPrincipalView : UserControl
    {
        public LoginPrincipalView()
        {
            InitializeComponent();
        } 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void pwb0_LostFocus(object sender, RoutedEventArgs e)
        {
            txtPassword.Text = "";
            txtPassword.Text = pwb0.Password; 
        }
    }
}
