using AdHr.ViewModels;
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

namespace AdHr.Views.AdhrUser
{
    /// <summary>
    /// Interaction logic for Create.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {
        public CreateWindow(AdhrUserViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        private void CreateSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
