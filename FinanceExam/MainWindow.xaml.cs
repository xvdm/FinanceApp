using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
    [Serializable]
    public class Card
    {

        public List<History_Data> _dataGrid = null; // список для таблицы в разделе "история"

        public List<Category_Data> _dataGridCategories = null; // список для таблице в разделе "график"

        public List<History_Data> _FilterGrid = null; // список фильтров для таблицы в разделе "история"

        public Dictionary<string, int> _diagramData = new Dictionary<string, int>(); // категория и ее сумма денег

        //[NonSerialized]
        public List<string> _categoryColor = new List<string>(); // соответствие строк и цветов (Red = Brushes.Red и тд)

        public History_Data LastAddedData = new History_Data(null, 0, null, null);

        public bool LastAddedDataIsCorrect = true;

        public double Balance { get; set; }

        public string Name { get; set; }

        public Card(string name)
        {
            Name = name;
        }
    }

    public partial class MainWindow : Window
    { 
        private bool _expanded = false;
        int MaxDiagramCanvasSize = 600;
        int MinDiagramCanvasSize = 450;

       FileProcessing _fileData = new FileProcessing(); // работа с сохранением/извлечением из файла

        List<Categories> _dataSettingCategory = null;

        public string GeneralBallanceChange
        {
            get
            {
                return GeneralBalance.Content.ToString();
            }
            set
            {
                GeneralBalance.Content = value;
            }
        }

        public List<Card> Cards;

        public int CurrentCardIndex;

        public MainWindow()
        {
            InitializeComponent();
            Currency.Content = "₴";
            DiagramCanvas.Width = MaxDiagramCanvasSize;
            DiagramCanvas.Height = MaxDiagramCanvasSize;
            this.Height = System.Windows.SystemParameters.WorkArea.Height / 1.2;
            this.Width = System.Windows.SystemParameters.WorkArea.Width / 1.2;

            _dataSettingCategory = _fileData.LoadCategoryData();

            Cards = _fileData.LoadCardData();


            if (Cards == null)
            {
                Cards = new();
            }
            CardsComboBox.ItemsSource = Cards;
            CardsComboBox.SelectedItem = Cards[0];
            CardsComboBox.DisplayMemberPath = "Name";


            CurrentCardIndex = 0;


            


            DrawCircleDiagram();
            UpDateBallance();
        }

        private void AddCard(string name) 
        {
            var card = new Card(name);
            var cmb = new ComboBoxItem();
            Cards.Add(card);
            cmb.Content = card.Name;
            CardsComboBox.Items.Add(cmb);
            if(CardsComboBox.Items.Count == 1) 
                ((ComboBoxItem)(CardsComboBox.Items.GetItemAt(0))).IsSelected = true;
        }


        private void CardsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentCardIndex = CardsComboBox.SelectedIndex;



            DrawCircleDiagram();
            UpdateTables();
            UpDateBallance();
            
        }

        private void UpdateTables()
        {
            Datagrid.ItemsSource = Cards[CurrentCardIndex]._dataGrid;
            Datagrid.Items.Refresh();

            DatagridCategory.ItemsSource = Cards[CurrentCardIndex]._dataGridCategories;
            DatagridCategory.Items.Refresh();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

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
            WindowSetting WinSet = new WindowSetting(_dataSettingCategory, Cards[CurrentCardIndex]._dataGrid, Cards[CurrentCardIndex]._dataGridCategories, Cards);
            WinSet.ShowDialog();
            CardsComboBox.Items.Refresh();
            DatagridCategory.Items.Refresh();
            Datagrid.Items.Refresh();
            _fileData.SaveCardData(Cards);
        }

        private void Button_AddInData(object sender, RoutedEventArgs e)
        {
            if (Cards.Count > 0)
            {
                NewDataItem ItemDialog = new NewDataItem(_dataSettingCategory);
                ItemDialog.Owner = this;
                ItemDialog.ShowDialog();

                if (Cards[CurrentCardIndex].LastAddedDataIsCorrect == true)
                {
                    GeneralBalance.Content = Cards[CurrentCardIndex].Balance;

                    HistoryTableEdit(Cards[CurrentCardIndex].LastAddedData);
                    ChartTableEdit(Cards[CurrentCardIndex].LastAddedData);
                    DiagramEdit(Cards[CurrentCardIndex].LastAddedData);

                    DrawCircleDiagram();
                }
            }
            else
            {
                MessageBox.Show("Нет ни одного счета");
            }
            _fileData.SaveCardData(Cards);
        }

        private void UpDateBallance()
        {
            double buffballace = 0;
            if(Cards != null)
            {
                if (Cards[CurrentCardIndex]._dataGrid != null)
                {
                    for (int i = 0; i < Cards[CurrentCardIndex]._dataGrid.Count; i++)
                    {
                        buffballace += Cards[CurrentCardIndex]._dataGrid[i].Money;
                    }
                }
                GeneralBallanceChange = buffballace.ToString();
            }

        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            NewDataItem ItemDialog = new NewDataItem(_dataSettingCategory);
            ItemDialog.Owner = this;
            ItemDialog.ShowDialog();

            if (Cards[CurrentCardIndex].LastAddedDataIsCorrect == true)
            {
                GeneralBalance.Content = Cards[CurrentCardIndex].Balance;

                HistoryTableEdit(Cards[CurrentCardIndex].LastAddedData);
                ChartTableEdit(Cards[CurrentCardIndex].LastAddedData);
                DiagramEdit(Cards[CurrentCardIndex].LastAddedData);

                DrawCircleDiagram();
            }

        }

        public void AddMoneyToGeneralBalance(double money)
        {
            Cards[CurrentCardIndex].Balance += money;
        }

        private void HistoryTableEdit(History_Data data)
        {
            Random r = new Random();
            if (Cards[CurrentCardIndex]._dataGrid == null)
            {
                Cards[CurrentCardIndex]._dataGrid = new List<History_Data>();
                Datagrid.ItemsSource = Cards[CurrentCardIndex]._dataGrid;
            }

            Cards[CurrentCardIndex]._dataGrid.Add(new History_Data(data.Day, data.Money, data.Category, data.Comment));

            Datagrid.Items.Refresh();
        }

        private void ChartTableEdit(History_Data data)
        {
            if (Cards[CurrentCardIndex]._dataGridCategories == null)
            {
                Cards[CurrentCardIndex]._dataGridCategories = new List<Category_Data>();
                DatagridCategory.ItemsSource = Cards[CurrentCardIndex]._dataGridCategories;
            }

            string category = data.Category;
            string color = null;
            foreach (var col in _dataSettingCategory)
            {
                if (col.Category == category)
                {
                    color = col.Color;
                    break;
                }
            }    

            int money = Convert.ToInt32(data.Money);            

            if (Cards[CurrentCardIndex]._diagramData.ContainsKey(category)) // если категория уже есть
            {
                foreach (var x in Cards[CurrentCardIndex]._dataGridCategories) // в таблице (в разделе "график") ищу строку с этой категорией
                {
                    if (x.Category == category)
                    {
                        x.Money += money; // и увеличиваю сумму в этой строке
                    }
                }
            }
            else
            {
                Cards[CurrentCardIndex]._dataGridCategories.Add(new Category_Data(category, color, money)); // добавление новой категории, ее цвета и кол-во денег в таблицу
                Cards[CurrentCardIndex]._categoryColor.Add(color);
            }

            DatagridCategory.Items.Refresh();
        }

        private void DiagramEdit(History_Data data)
        {
            string category = data.Category;
            int money = Convert.ToInt32(data.Money);
            if (Cards[CurrentCardIndex]._diagramData.ContainsKey(category)) // если категория уже есть
                Cards[CurrentCardIndex]._diagramData[category] += money; // увеличиваю кол-во денег в ней
            else
                Cards[CurrentCardIndex]._diagramData.Add(category, money); // добавление новой категории в диаграмму
        }

        private void Button_Click_Expand(object sender, RoutedEventArgs e)
        {
            if (_expanded == true)
            {
                this.WindowState = WindowState.Maximized;
                DiagramCanvas.Width = MaxDiagramCanvasSize;
                DiagramCanvas.Height = MaxDiagramCanvasSize;
            }
            else
            {
                this.WindowState = WindowState.Normal;
                DiagramCanvas.Width = MinDiagramCanvasSize;
                DiagramCanvas.Height = MinDiagramCanvasSize;
            }

            DrawCircleDiagram();
            _expanded = !_expanded;
        }

        private void DrawCircleDiagram()
        {
            DiagramCanvas.Children.Clear();
            if(Cards != null)
            {
                if (Cards[CurrentCardIndex]._diagramData.Count > 1)
                {
                    int[] data = new int[Cards[CurrentCardIndex]._diagramData.Count];
                    Brush[] brushes = new Brush[Cards[CurrentCardIndex]._categoryColor.Count];
                    int i = 0;
                    foreach (var x in Cards[CurrentCardIndex]._diagramData)
                    {
                        data[i] = x.Value;
                        i++;
                    }
                    i = 0;
                    foreach (var x in Cards[CurrentCardIndex]._categoryColor)
                    {
                        TypeConverter tc = TypeDescriptor.GetConverter(typeof(Color)); // добавление соответствия между строкой (с названием цвета) и цветом (brush)
                        Color clr = (Color)tc.ConvertFromString(x);
                        Brush brush = new SolidColorBrush(clr);
                        brushes[i] = brush;
                        i++;
                    }
                    var sum = data.Sum();
                    var angles = data.Select(d => d * 2.0 * Math.PI / sum);

                    double radius = DiagramCanvas.Width / 2;
                    double startAngle = 0.0;

                    var centerPoint = new Point(radius, radius);
                    var xyradius = new Size(radius, radius);

                    i = 0;
                    foreach (var angle in angles)
                    {
                        var endAngle = startAngle + angle;

                        var startPoint = centerPoint;
                        startPoint.Offset(radius * Math.Cos(startAngle), radius * Math.Sin(startAngle));

                        var endPoint = centerPoint;
                        endPoint.Offset(radius * Math.Cos(endAngle), radius * Math.Sin(endAngle));

                        var angleDeg = angle * 180.0 / Math.PI;

                        System.Windows.Shapes.Path p = new System.Windows.Shapes.Path()
                        {
                            Stroke = Brushes.Black,
                            Fill = brushes[i++],
                            StrokeThickness = 1,
                            Data = new PathGeometry(
                                new PathFigure[]
                                {
                                new PathFigure(
                                    centerPoint,
                                    new PathSegment[]
                                    {
                                        new LineSegment(startPoint, isStroked: true),
                                        new ArcSegment(endPoint, xyradius,
                                        angleDeg, angleDeg > 180,
                                        SweepDirection.Clockwise, isStroked: true)
                                    },
                                    closed: true)
                                })
                        };
                        DiagramCanvas.Children.Add(p);

                        startAngle = endAngle;
                    }
                }
                else if (Cards[CurrentCardIndex]._diagramData.Count == 1)
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = DiagramCanvas.Width;
                    ellipse.Height = DiagramCanvas.Height;

                    TypeConverter tc = TypeDescriptor.GetConverter(typeof(Color)); // добавление соответствия между строкой (с названием цвета) и цветом (brush)
                    Color clr = (Color)tc.ConvertFromString(Cards[CurrentCardIndex]._categoryColor[0]);
                    Brush brush = new SolidColorBrush(clr);
                    ellipse.Fill = brush;

                    ellipse.Stroke = Brushes.Black;
                    ellipse.StrokeThickness = 1;
                    DiagramCanvas.Children.Add(ellipse);
                }
            }

        }



        private void Button_Filter(object sender, RoutedEventArgs e)
        {

            Cards[CurrentCardIndex]._FilterGrid = new();
            if (searchBox.Text.Equals(""))
            {
                Cards[CurrentCardIndex]._FilterGrid.AddRange(Cards[CurrentCardIndex]._dataGrid);
            }
            else
            {
                foreach (History_Data row in Cards[CurrentCardIndex]._dataGrid)
                {
                    if (row.Category.Contains(searchBox.Text) || row.Day.Contains(searchBox.Text) || row.Comment.Contains(searchBox.Text))
                    {
                        Cards[CurrentCardIndex]._FilterGrid.Add(row);
                    }
                }
            }
            Datagrid.ItemsSource = Cards[CurrentCardIndex]._FilterGrid;
            Datagrid.Items.Refresh();
        }
    }

    [Serializable]
    public class History_Data
    {
        public History_Data(string day, double money, string category, string comment)
        {
            Day = day;
            Money = money;
            Category = category;
            Comment = comment;
        }

        public string Day { get; set; }

        public double Money { get; set; }

        public string Category { get; set; }

        public string Comment { get; set; }
    }

    [Serializable]
    public class Category_Data
    {
        public Category_Data(string category, string color, int money)
        {
            Category = category;
            Color = color;
            Money = money;
        }

        public string Category { get; set; }

        public string Color { get; set; }

        public double Money { get; set; }
    }

    [Serializable]
    public class Categories
    {
        public Categories(string category, string color)
        {
            Category = category;
            Color = color;
        }

        public string Category { get; set; }

        public string Color { get; set; }
    }

    public class FileProcessing
    {
        private static BinaryFormatter binaryFormatter;
        private readonly string _path = @"..\Data";
        private readonly string _settingspath = @"..\..\..\SettingsCategory";
        public FileProcessing()
        {
            binaryFormatter = new BinaryFormatter();
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
        }

        public List<Categories> LoadCategoryData()
        {
            using (Stream stream = File.Open(_settingspath + "\\" + "SettingsCategory.txt", FileMode.OpenOrCreate))
            {
                if (stream.Length > 0)
                {
                    return (List<Categories>)binaryFormatter.Deserialize(stream);
                }
                else
                {
                    return null;
                }
            }
        }

        public void SaveCategory(List<Categories> data)
        {
            using (Stream stream = File.Open(_settingspath + "\\" + "SettingsCategory.txt", FileMode.OpenOrCreate))
            {
                try
                {
                    binaryFormatter.Serialize(stream, data);
                }
                catch (SerializationException e)
                {
                    MessageBox.Show("Ошибка сохранения. Причина: " + e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                }
            }
        }

        public void SaveCardData(List<Card> data)
        {
            using (Stream stream = File.Open(_path + '\\' + "CardData.txt", FileMode.Open))
            {
                try
                {
                    binaryFormatter.Serialize(stream, data);
                }
                catch (SerializationException e)
                {
                    MessageBox.Show("Ошибка сохранения. Причина: " + e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                }
            }
        }

        public List<Card> LoadCardData()
        {
            using (Stream stream = File.Open(_path + '\\' + "CardData.txt", FileMode.OpenOrCreate))
            {
                if (stream.Length > 0)
                {
                    stream.Position = 0;
                    return (List<Card>)binaryFormatter.Deserialize(stream);
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
