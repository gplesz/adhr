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
using System.Windows.Shapes;

namespace AdHr.Views.AdhrUser
{
    /// <summary>
    /// Interaction logic for Read.xaml
    /// </summary>
    public partial class ReadWindow : Window
    {
        public ReadWindow()
        {
            InitializeComponent();
        }

        public ReadWindow(AdhrUserViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }

        public AdhrUserViewModel ViewModel { get { return (AdhrUserViewModel)DataContext; } }

        private void PropertyGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid.SelectedValue is AdhrPropertyViewModel selectedProperty)
            {
                ViewModel.SelectedProperty = new ObservableCollection<AdhrValueViewModel>(selectedProperty.Values);
            }
            else
            {
                ViewModel.SelectedProperty = new ObservableCollection<AdhrValueViewModel>();
            }
        }
    }
}
