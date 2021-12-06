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
        public MainWindow()
        {
            InitializeComponent();
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

        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            
            List<History> result = new List<History>();
            result.Add(new History("06.12.2021", 600, "Thriller", ""));
            result.Add(new History("06.12.2021", 700, "Back in Black", ""));
            result.Add(new History("06.12.2021", -10, "Saturday Night Fever", ""));
            result.Add(new History("06.12.2021", 30, "The Dark Side of the Moon", ""));
            grid.ItemsSource = result;
        }
    }

    public class History
    {
        public History(string Id, double Vocalist, string Album, string Year)
        {
            this.Day = Id;
            this.Money = Vocalist;
            this.Category = Album;
            this.Comment = Year;
        }

        public string Day { get; set; }
        public double Money { get; set; }
        public string Category { get; set; }
        public string Comment { get; set; }
    }

}
