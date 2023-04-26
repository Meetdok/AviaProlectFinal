﻿using System;
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
using WebProject.WebModels;
using WpfProject.ViewModels;

namespace WpfProject.Windows
{
    /// <summary>
    /// Логика взаимодействия для FlightsOrderEdit.xaml
    /// </summary>
    public partial class FlightsOrderEdit : Window
    {
        public FlightsOrderEdit(Flight flight)
        {
            InitializeComponent();
            DataContext = new EditFlightVM(flight);
        }       
    }
}
