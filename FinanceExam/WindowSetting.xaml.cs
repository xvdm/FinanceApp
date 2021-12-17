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
    /// Interaction logic for WindowSetting.xaml
    /// </summary>
    public partial class WindowSetting : Window
    {

        private List<Categories> SettinhCategory = null;
        private List<Categories> FilterList = null;
        private List<History_Data> dataGrid = null;
        private List<Category_Data> dataGridCategories = null;
        private List<Card> SettingCard = null;

        private bool settiggrow = false;
        private bool settigcard = false;

        public WindowSetting(List<Categories> _SettinhCategory, List<History_Data> _dataGrid, List<Category_Data>_dataGridCategories, List<Card> _SettingCard)
        {
            InitializeComponent();
            SettinhCategory = _SettinhCategory;
            SettingCard = _SettingCard;
            dataGrid = _dataGrid;
            dataGridCategories = _dataGridCategories;


            CategoryData.ItemsSource = SettinhCategory;
            CardData.ItemsSource = SettingCard;
        }

        public string CurrencyLable
        {
            get
            {
                return CurrencyBox.Text;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void Button_Card(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!settigcard)
                {
                    if (SettingNameCard.Text == "")
                    {
                        throw new ArgumentNullException();
                    }

                    foreach (var x in SettingCard)
                    {
                        if (x.Name == SettingNameCard.Text)
                        {
                            throw new Exception();
                        }
                    }
                    SettingCard.Add(new(SettingNameCard.Text));
                    //((MainWindow)Application.Current.MainWindow)AddCard(SettingNameCard.Text);
                }
                else
                {
                    int index = SettingCard.IndexOf((Card)CardData.SelectedItem);
                    SettingCard[index].Name = SettingNameCard.Text;
                    CardData.Items.Refresh();
                    settigcard = false;
                }


                SettingNameCard.Text = null;
                CardData.Items.Refresh();

                CardButton.Content = "Добавить";
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Ошибка, пустые значения", "Внимание");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Имя счета уже используются", "Внимание");
            }
        }

        private void Button_Card_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CardData.Items.Count < 2)
                    throw new Exception();

                SettingNameCard.Text = null;
                SettingCard.Remove((Card)CardData.SelectedItem);

                CardButtonDelete.IsEnabled = false;
                CardButton.Content = "Добавить";
                CardData.Items.Refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Вы не можете удалить последнюю карту.\nДолжна существовать хотя бы 1 карта" , "Внимание");
                SettingNameCard.Text = null;
                CardButtonDelete.IsEnabled = false;
                CardButton.Content = "Добавить";
            }

        }


        private void Row_DoubleClick_Card(object sender, MouseButtonEventArgs e)
        {
            settigcard = true;
            CardButtonDelete.IsEnabled = true;
            CardButton.Content = "Изменить";

            Card temp = (Card)CardData.SelectedItem;

            SettingNameCard.Text = temp.Name;
        }



        private void Button_Color(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!settiggrow)
                {
                    if (SettingNameCategory.Text == "" || ColorPick.SelectedColor == null)
                    {
                        throw new ArgumentNullException();
                    }

                    foreach (var x in SettinhCategory)
                    {
                        if (x.Category == SettingNameCategory.Text || x.Color == ColorPick.SelectedColorText)
                        {
                            throw new Exception();
                        }
                    }
                    SettinhCategory.Add(new(SettingNameCategory.Text, ColorPick.SelectedColorText));

                }
                else
                {
                    int index = SettinhCategory.IndexOf((Categories)CategoryData.SelectedItem);
                    SettinhCategory[index].Category = SettingNameCategory.Text;
                    SettinhCategory[index].Color = ColorPick.SelectedColorText;
                    CategoryData.Items.Refresh();
                    settiggrow = false;
                }


                SettingNameCategory.Text = null;
                ColorPick.SelectedColor = null;
                CategoryData.Items.Refresh();

                ResetButton.Content = "Сбросить";
                ColorButton.Content = "Добавить";
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Ошибка, пустые значения", "Внимание");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Цвет или имя категории уже используются", "Внимание");
            }

        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            settiggrow = true;
            ColorButton.Content = "Изменить";
            ResetButton.Content = "Удалить";

            Categories temp = (Categories)CategoryData.SelectedItem;

            SettingNameCategory.Text = temp.Category;
            ColorPick.SelectedColor = (Color)ColorConverter.ConvertFromString(temp.Color);
        }

        private void Button_Reset(object sender, RoutedEventArgs e)
        {
            SettingNameCategory.Text = null;
            ColorPick.SelectedColor = null;

            SettinhCategory.Remove((Categories)CategoryData.SelectedItem);

            ResetButton.Content = "Сбросить";
            ColorButton.Content = "Добавить";
            CategoryData.Items.Refresh();

        }

        private void Button_Filter(object sender, RoutedEventArgs e)
        {

            FilterList = new();
            if (searchBox.Text.Equals(""))
            {
                FilterList.AddRange(SettinhCategory);
            }
            else
            {
                foreach (Categories row in SettinhCategory)
                {
                    if (row.Category.Contains(searchBox.Text))
                    {
                        FilterList.Add(row);
                    }
                }
            }
            CategoryData.ItemsSource = FilterList;
            CategoryData.Items.Refresh();

        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

            ChengeBallance();
            this.Close();
        }


        private void ChengeBallance()
        {
            double ballance = Convert.ToDouble(((MainWindow)Application.Current.MainWindow).GeneralBalance.Content);
            char symbol = ' ';
            double kurs = 1;

            if (((MainWindow)Application.Current.MainWindow).Currency.Content == "₴" && CurrencyLable == "$")
            {
                symbol = '/';
                kurs = 27.24;
                ((MainWindow)Application.Current.MainWindow).Currency.Content = "$";
            }
            else if (((MainWindow)Application.Current.MainWindow).Currency.Content == "₴" && CurrencyLable == "€")
            {
                symbol = '/';
                kurs = 30.34;
                ((MainWindow)Application.Current.MainWindow).Currency.Content = "€";
            }

            else if(((MainWindow)Application.Current.MainWindow).Currency.Content == "$" && CurrencyLable == "₴")
            {
                symbol = '*';
                kurs = 27.24;
                ((MainWindow)Application.Current.MainWindow).Currency.Content = "₴";
            }
            else if (((MainWindow)Application.Current.MainWindow).Currency.Content == "€" && CurrencyLable == "₴")
            {
                symbol = '*';
                kurs = 30.34;
                ((MainWindow)Application.Current.MainWindow).Currency.Content = "₴";
            }

            else if(((MainWindow)Application.Current.MainWindow).Currency.Content == "$" && CurrencyLable == "€")
            {
                symbol = '*';
                kurs = 0.88;
                ((MainWindow)Application.Current.MainWindow).Currency.Content = "€";
            }
            else if (((MainWindow)Application.Current.MainWindow).Currency.Content == "€" && CurrencyLable == "$")
            {
                symbol = '*';
                kurs = 1.132;
                ((MainWindow)Application.Current.MainWindow).Currency.Content = "$";
            }

            switch (symbol)
            {
                case '*':
                    for (int i = 0; i < dataGrid.Count; i++)
                    {
                        dataGrid[i].Money = dataGrid[i].Money * kurs;
                    }
                    for (int i = 0; i < dataGridCategories.Count; i++)
                    {
                        dataGridCategories[i].Money = dataGridCategories[i].Money / kurs;
                    }
                    ((MainWindow)Application.Current.MainWindow).GeneralBalance.Content = ballance * kurs;
                    break;
                case '/':
                    for (int i = 0; i < dataGrid.Count; i++)
                    {
                        dataGrid[i].Money = dataGrid[i].Money / kurs;
                    }
                    for (int i = 0; i < dataGridCategories.Count; i++)
                    {
                        dataGridCategories[i].Money = dataGridCategories[i].Money / kurs;
                    }
                    ((MainWindow)Application.Current.MainWindow).GeneralBalance.Content = ballance / kurs;
                    break;
                default:
                    break;
            }

            Close();

        }
     }
}