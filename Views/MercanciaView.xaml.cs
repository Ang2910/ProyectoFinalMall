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
    /// Lógica de interacción para MercanciaView.xaml
    /// </summary>
    public partial class MercanciaView : UserControl
    {
        public MercanciaView()
        {
            InitializeComponent();
        } 
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Estas seguro de eliminar la mercancia?", "Confirme",
               MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                pvm.EliminarMercanciaCommand.Execute(null);
            }

        }
    }
}
