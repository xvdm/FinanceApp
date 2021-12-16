using System;
using System.Collections.Generic;
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
        private List<Categories> SettinhCategory = null;

        public NewDataItem(List<Categories> _SettinhCategory)
        {
            InitializeComponent();
            SettinhCategory = _SettinhCategory;

            foreach(Categories x in _SettinhCategory)
            {
                InputCategory.Items.Add(x.Category);
            }

            if (((MainWindow)Application.Current.MainWindow).Cards[((MainWindow)Application.Current.MainWindow).CurrentCardIndex].LastAddedDataIsCorrect == false)
            {
                InputDate.Text = ((MainWindow)Application.Current.MainWindow).Cards[((MainWindow)Application.Current.MainWindow).CurrentCardIndex].LastAddedData.Day;
                InputMoney.Text = ((MainWindow)Application.Current.MainWindow).Cards[((MainWindow)Application.Current.MainWindow).CurrentCardIndex].LastAddedData.Money.ToString();
                InputCategory.Text = ((MainWindow)Application.Current.MainWindow).Cards[((MainWindow)Application.Current.MainWindow).CurrentCardIndex].LastAddedData.Category;
                InputComment.Text = ((MainWindow)Application.Current.MainWindow).Cards[((MainWindow)Application.Current.MainWindow).CurrentCardIndex].LastAddedData.Comment;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Cards[((MainWindow)Application.Current.MainWindow).CurrentCardIndex].LastAddedDataIsCorrect = false;
            Close();
        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Cards[((MainWindow)Application.Current.MainWindow).CurrentCardIndex].LastAddedDataIsCorrect = false;
            Close();
        }

        private void Button_Click_ADD(object sender, RoutedEventArgs e)
        {
            string moneyPattern = @"^([1-9]{1}[0-9]{0,2}(\,\d{3})*(,\d{0,2})?|[1-9]{1}\d{0,}(,\d{0,2})?|0(,\d{0,2})?|(,\d{1,2}))$|^\-?\$?([1-9]{1}\d{0,2}(\,\d{3})*(,\d{0,2})?|[1-9]{1}\d{0,}(,\d{0,2})?|0(,\d{0,2})?|(,\d{1,2}))$|^\(\$?([1-9]{1}\d{0,2}(\,\d{3})*(,\d{0,2})?|[1-9]{1}\d{0,}(,\d{0,2})?|0(,\d{0,2})?|(,\d{1,2}))\)$";
            if (Regex.IsMatch(InputMoney.Text, moneyPattern) == false || InputDate.Text == "" || InputCategory.Text == "" || InputComment.Text == "") {
                ((MainWindow)Application.Current.MainWindow).Cards[((MainWindow)Application.Current.MainWindow).CurrentCardIndex].LastAddedDataIsCorrect = false;
                if (Regex.IsMatch(InputMoney.Text, moneyPattern) == false) 
                    InputMoney.Text = "0";
                MessageBox.Show("Incorrect data.");
            }
            else
            {
                ((MainWindow)Application.Current.MainWindow).Cards[((MainWindow)Application.Current.MainWindow).CurrentCardIndex].LastAddedDataIsCorrect = true;
                ((MainWindow)Application.Current.MainWindow).AddMoneyToGeneralBalance(Convert.ToDouble(InputMoney.Text));
            }
            ((MainWindow)Application.Current.MainWindow).Cards[((MainWindow)Application.Current.MainWindow).CurrentCardIndex].LastAddedData = new History_Data(InputDate.Text, Convert.ToDouble(InputMoney.Text), InputCategory.Text, InputComment.Text);
            Close();
        }
    }
}
