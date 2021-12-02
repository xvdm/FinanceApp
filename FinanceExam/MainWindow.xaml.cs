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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinanceExam
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
        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RollUp_Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Expenses_Button_Click(object sender, RoutedEventArgs e)
        {
            Income_Button.Opacity = 0.5;
            Expenses_Button.Opacity = 1;
        }

        private void Income_Button_Click(object sender, RoutedEventArgs e)
        {
            Income_Button.Opacity = 1;
            Expenses_Button.Opacity = 0.5;
        }

    }
}
