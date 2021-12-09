using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FinanceExam
{
    /// <summary>
    /// Interaction logic for NewDataItem.xaml
    /// </summary>
    public partial class NewDataItem : Window
    {
        public History_Data Item { private set;  get; }

        public NewDataItem()
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


        private void Button_Click_ADD(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).ConfUser.AddItem(new History_Data(InputData.Text,Convert.ToDouble(InputMoney.Text), InputCategory.Text,InputComment.Text));
            ((MainWindow)Application.Current.MainWindow).ConfUser.Balance = Convert.ToDouble(InputMoney.Text) + ((MainWindow)Application.Current.MainWindow).ConfUser.Balance;

            Close();

        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
