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
    /// Interaction logic for NewCard.xaml
    /// </summary>
    public partial class NewCard : Window
    {
        public NewCard()
        {
            InitializeComponent();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_ADD(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Cards.Add(new Card(NameTextBox.Text));
            var cmb = new ComboBoxItem();
            cmb.Content = NameTextBox.Text;
            ((MainWindow)Application.Current.MainWindow).CardsComboBox.Items.Add(cmb);
            ((MainWindow)Application.Current.MainWindow).CardsComboBox.Items.Refresh();
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();
    }
}
