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
    /// 

    public partial class MainWindow : Window
    {
        private bool _expanded = false;
        User MainUser = new User();

        public User ConfUser { get { return MainUser; } }


        public MainWindow()
        {
            InitializeComponent();

            this.Height = System.Windows.SystemParameters.WorkArea.Height / 1.2;
            this.Width = System.Windows.SystemParameters.WorkArea.Width / 1.2;
  
            Datagrid.ItemsSource = MainUser.Data;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void Button_Click_Expand(object sender, RoutedEventArgs e)
        {
            if (_expanded == true)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;

            _expanded = !_expanded;
        }


        private void Button_Click_Roll(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        private void Button_Setting(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("In coming future", "Setting");
        }


        private void Сurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            
        }

        private void Button_AddInData(object sender, RoutedEventArgs e)
        {

            NewDataItem ItemDialog = new NewDataItem();
            ItemDialog.Owner = this;
            ItemDialog.ShowDialog();

            GeneralBalance.Content = MainUser.Balance;
            Datagrid.Items.Refresh();
        }
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

    public class User
    {
        double UserMoney = 0;

        List<History_Data> DATAGrid;
        List<Category> CategoryList;


        private void CardHistory()
        {

        }


        private void LoadCategory()
        {
            string path = @"C:\Users\Isae_j3yu\source\repos\FinanceExam\FinanceExam\Category\Category.txt";
            
        }

        public User()
        {
            DATAGrid = new List<History_Data>();
            LoadCategory();
            CardHistory();
        }

        public List<History_Data> Data
        {
            set
            {
                DATAGrid = value;
            }

            get
            {
                return DATAGrid;
            }
        }

        public double Balance { set { UserMoney = value; } get { return UserMoney; } }

        public void AddItem(History_Data NewItem)
        {
            DATAGrid.Add(NewItem);
        }

        public void AddCategory()
        {

        }

    }

    struct Category
    {
        string Name;
        string Color;
    }
    
}

