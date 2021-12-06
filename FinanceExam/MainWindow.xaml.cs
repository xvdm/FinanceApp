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

namespace FinanceExam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _expanded = false;

        public MainWindow()
        {
            InitializeComponent();
            this.Height = System.Windows.SystemParameters.WorkArea.Height / 1.2;
            this.Width = System.Windows.SystemParameters.WorkArea.Width / 1.2;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_Roll(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click_Expand(object sender, RoutedEventArgs e)
        {
            if (_expanded == true)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
             
            _expanded = !_expanded;
        }
    }
}
