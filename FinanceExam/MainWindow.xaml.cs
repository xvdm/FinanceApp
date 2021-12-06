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
        List<History_Data> data_grid = null;
        TransverSetting Transver = new TransverSetting();
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


        private void Button_Setting(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("In coming future", "Setting");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(data_grid == null)
            {
                data_grid = new List<History_Data>();
                Datagrid.ItemsSource = data_grid;
            }
            data_grid.Add(new History_Data("06.12.2021", 600, "Вадим", "Диме"));



            Transver.General_Balance += 600;
            GeneralBalance.Content = Transver.General_Balance;

            Datagrid.Items.Refresh();
        }

        private void Сurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if(Currency.SelectedItem .ToString() == "$")
            {
                double temp = 0;
                temp = Transver.General_Balance / 27.37;
                GeneralBalance.Content = temp;
            }
        }
    }

    public class TransverSetting
    {
        private double MONEY = 0;


        public double General_Balance { get { return MONEY; } set { MONEY = value; } }


    }

    public class History_Data
    {

        private string day;
        private double money;
        private string category;
        private string comment;

        public History_Data(string _day, double _money, string _category, string _comment)
        {
            this.day = _day;
            this.money = _money;
            this.category = _category;
            this.comment = _comment;
        }

        public string Day { get { return this.day; } set { this.day = value; } }
        public double Money { get { return this.money; } set { this.money = value; } }
        public string Category { get { return this.category; } set { this.category = value; } }
        public string Comment { get { return this.comment; } set { this.comment = value; } }



    }
}
