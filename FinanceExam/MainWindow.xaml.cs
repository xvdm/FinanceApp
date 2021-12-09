using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class MainWindow : Window
    {
        private List<History_Data> _dataGrid = null; // список для таблицы в разделе "история"

        private List<Category_Data> _dataGridCategories = null; // список для таблице в разделе "график"

        private Dictionary<string, int> _diagramData = new Dictionary<string, int>(); // категория и ее сумма денег

        private Dictionary<string, Brush> _categoryColor = new Dictionary<string, Brush>(); // соответствие строк и цветов (Red = Brushes.Red и тд)

        private TransverSetting Transver = new TransverSetting();

        private bool _expanded = false;

        public MainWindow()
        {
            InitializeComponent();
            this.Height = System.Windows.SystemParameters.WorkArea.Height / 1.2;
            this.Width = System.Windows.SystemParameters.WorkArea.Width / 1.2;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void Button_Click_Exit(object sender, RoutedEventArgs e) => Close();

        private void Button_Click_Roll(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

        private void Button_Setting(object sender, RoutedEventArgs e) => MessageBox.Show("In coming future", "Setting");

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            int money = r.Next(100, 2000);

            Transver.General_Balance += money;
            GeneralBalance.Content = Transver.General_Balance;

            string category = "category";
            int random_category = r.Next(1, 5);
            category += random_category.ToString(); // добавляю рандомную цифру в конец, чтобы было несколько разных категорий

            HistoryTableEdit(money, category);
            ChartTableEdit(money, category);
            DiagramEdit(money, category);
            
            DrawCircleDiagram();
        }

        private void HistoryTableEdit(int money, string category)
        {
            Random r = new Random();
            if (_dataGrid == null)
            {
                _dataGrid = new List<History_Data>();
                Datagrid.ItemsSource = _dataGrid;
            }

            string day = "day";
            string comment = "comment";
            
            _dataGrid.Add(new History_Data(day, money, category, comment));

            Datagrid.Items.Refresh();
        }

        private void ChartTableEdit(int money, string category)
        {
            if (_dataGridCategories == null)
            {
                _dataGridCategories = new List<Category_Data>();
                DatagridCategory.ItemsSource = _dataGridCategories;
            }

            string color = "Gray"; // эта переменная будет инициализироваться выбором пользователя при создании категории
            switch (category[category.Length - 1]) // пока нет функционала - цвет для каждой категории задается тут (в зависимости от рандомной цифры в конце названия категории)
            {
                case '1': color = "Gray"; break;
                case '2': color = "Red"; break;
                case '3': color = "Green"; break;
                case '4': color = "Yellow"; break;
            }

            if (_diagramData.ContainsKey(category)) // если категория уже есть
            {
                foreach (var x in _dataGridCategories) // в таблице (в разделе "график") ищу строку с этой категорией
                {
                    if (x.Category == category)
                    {
                        x.Money += money; // и увеличиваю сумму в этой строке
                    }
                }
            }
            else
            {
                _dataGridCategories.Add(new Category_Data(category, color, money)); // добавление новой категории, ее цвета и кол-во денег в таблицу

                TypeConverter tc = TypeDescriptor.GetConverter(typeof(Color)); // добавление соответствия между строкой (с названием цвета) и цветом (brush)
                Color clr = (Color)tc.ConvertFromString(color);
                Brush brush = new SolidColorBrush(clr);
                _categoryColor.Add(category, brush);
            }

            DatagridCategory.Items.Refresh();
        }

        private void DiagramEdit(int money, string category)
        {
            if (_diagramData.ContainsKey(category)) // если категория уже есть
                _diagramData[category] += money; // увеличиваю кол-во денег в ней
            else
                _diagramData.Add(category, money); // добавление новой категории в диаграмму
        }

        private void Сurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Currency.SelectedItem.ToString() == "$")
            {
                double temp = 0;
                temp = Transver.General_Balance / 27.37;
                GeneralBalance.Content = temp;
            }
        }

        private void Button_Click_Expand(object sender, RoutedEventArgs e)
        {
            if (_expanded == true)
            {
                this.WindowState = WindowState.Maximized;
                DiagramCanvas.Width = 300;
                DiagramCanvas.Height = 300;
            }
            else
            {
                this.WindowState = WindowState.Normal;
                DiagramCanvas.Width = 250;
                DiagramCanvas.Height = 250;
            }

            DrawCircleDiagram();
            _expanded = !_expanded;
        }

        private void DrawCircleDiagram()
        {
            DiagramCanvas.Children.Clear();

            if (_diagramData.Count > 1)
            {
                int[] data = new int[_diagramData.Count];
                Brush[] brushes = new Brush[_categoryColor.Count];
                int i = 0;
                foreach(var x in _diagramData)
                {
                    data[i] = x.Value;
                    i++;
                }
                i = 0;
                foreach (var x in _categoryColor)
                {
                    brushes[i] = x.Value;
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

                    Path p = new Path()
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
            else
            {
                Ellipse ellipse = new Ellipse();
                ellipse.Width = DiagramCanvas.Width;
                ellipse.Height = DiagramCanvas.Height;
                ellipse.Fill = _categoryColor.Values.First();
                ellipse.Stroke = Brushes.Black;
                ellipse.StrokeThickness = 1;
                DiagramCanvas.Children.Add(ellipse);
            }
        }
    }

    public class TransverSetting
    {
        public double General_Balance { get; set; }
    }

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

        public int Money { get; set; }
    }
}
