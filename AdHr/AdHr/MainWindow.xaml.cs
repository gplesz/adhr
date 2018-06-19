﻿using AdHr.ViewModels;
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

        public MainViewModel ViewModel
        {
            get
            {
                return (MainViewModel)DataContext;
            }
                
        }

        private void UserGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid.SelectedValue is AdhrUserViewModel selectedUser)
            {
                ViewModel.SelectedUserProperties = new ObservableCollection<AdhrPropertyViewModel>(selectedUser.Properties);
            }
            else
            {
                ViewModel.SelectedUserProperties = new ObservableCollection<AdhrPropertyViewModel>();
            }
        }

        private void PropertyGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid.SelectedValue is AdhrPropertyViewModel selectedProperty)
            {
                ViewModel.SelectedUserSelectedProperty = new ObservableCollection<AdhrValueViewModel>(selectedProperty.Values);
            }
            else
            {
                ViewModel.SelectedUserSelectedProperty = new ObservableCollection<AdhrValueViewModel>();
            }
        }
    }
}
