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
        private bool settingrow = false;
        public WindowSetting(List<Categories> _SettinhCategory)
        {
            InitializeComponent();
            SettinhCategory = _SettinhCategory;
            CategoryData.ItemsSource = SettinhCategory;
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Color(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!settingrow)
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
                    settingrow = false;
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
            settingrow = true;
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
    }
}