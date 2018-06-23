using AdHr.ViewModels.Settings;
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
using System.Windows.Shapes;

namespace AdHr.Views.Properties
{
    /// <summary>
    /// Interaction logic for PropertiesWindow.xaml
    /// </summary>
    public partial class PropertiesWindow : Window
    {
        public PropertiesWindow(SettingsViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        private void PropertiesSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
