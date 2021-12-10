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

            if(((MainWindow)Application.Current.MainWindow).LastAddedDataIsCorrect == false)
            {
                InputData.Text = ((MainWindow)Application.Current.MainWindow).LastAddedData.Day;
                InputMoney.Text = ((MainWindow)Application.Current.MainWindow).LastAddedData.Money.ToString();
                InputCategory.Text = ((MainWindow)Application.Current.MainWindow).LastAddedData.Category;
                InputComment.Text = ((MainWindow)Application.Current.MainWindow).LastAddedData.Comment;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void Button_Click_Exit(object sender, RoutedEventArgs e) => Close();

        private void Button_Click_Close(object sender, RoutedEventArgs e) => Close();

        private void Button_Click_ADD(object sender, RoutedEventArgs e)
        {
            if (InputData.Text == "" || InputMoney.Text == "" || InputCategory.Text == "" || InputComment.Text == "" || Convert.ToDouble(InputMoney.Text) <= 0) {
                ((MainWindow)Application.Current.MainWindow).LastAddedDataIsCorrect = false;
                if (InputMoney.Text == "" || Convert.ToDouble(InputMoney.Text) <= 0) InputMoney.Text = "0";
                ((MainWindow)Application.Current.MainWindow).LastAddedData = new History_Data(InputData.Text, Convert.ToDouble(InputMoney.Text), InputCategory.Text, InputComment.Text);
                MessageBox.Show("Incorrect data");
            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).ConfUser.AddItem(new History_Data(InputData.Text, Convert.ToDouble(InputMoney.Text), InputCategory.Text, InputComment.Text));
                ((MainWindow)Application.Current.MainWindow).ConfUser.Balance = Convert.ToDouble(InputMoney.Text) + ((MainWindow)Application.Current.MainWindow).ConfUser.Balance;
                ((MainWindow)Application.Current.MainWindow).LastAddedDataIsCorrect = true;
            }
            Close();
        }
    }
}
