using AdHr.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AdHr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UserGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            var selectedUser = grid.SelectedValue as AdhrUserViewModel;
            if (selectedUser!=null)
            {
                ((MainViewModel)DataContext).SelectedUserProperties = new ObservableCollection<AdhrPropertyViewModel>(selectedUser.Properties);
            }
            else
            {
                ((MainViewModel)DataContext).SelectedUserProperties = new ObservableCollection<AdhrPropertyViewModel>();
            }
        }

        private void PropertyGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            var selectedProperty = grid.SelectedValue as AdhrPropertyViewModel;
            if (selectedProperty != null)
            {
                ((MainViewModel)DataContext).SelectedUserSelectedProperty = new ObservableCollection<AdhrValueViewModel>(selectedProperty.Values);
            }
            else
            {
                ((MainViewModel)DataContext).SelectedUserSelectedProperty = new ObservableCollection<AdhrValueViewModel>();
            }
        }
    }
}
