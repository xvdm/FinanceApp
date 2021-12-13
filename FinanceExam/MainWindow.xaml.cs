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

        private bool _expanded = false;

        private User MainUser = new User();

        public History_Data LastAddedData = new History_Data(null, 0, null , null);

        public bool LastAddedDataIsCorrect = true;

        public MainWindow()
        {
            InitializeComponent();
            this.Height = System.Windows.SystemParameters.WorkArea.Height / 1.2;
            this.Width = System.Windows.SystemParameters.WorkArea.Width / 1.2;

            this.Height = System.Windows.SystemParameters.WorkArea.Height / 1.2;
            this.Width = System.Windows.SystemParameters.WorkArea.Width / 1.2;
  
            Datagrid.ItemsSource = MainUser.Data;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void Button_Click_Exit(object sender, RoutedEventArgs e) => Close();

        private void Button_Click_Roll(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

        private void Button_Setting(object sender, RoutedEventArgs e)
        {
            WindowSetting WinSet = new WindowSetting();
            WinSet.ShowDialog();
        }

        private void Button_AddInData(object sender, RoutedEventArgs e)
        {
            NewDataItem ItemDialog = new NewDataItem();
            ItemDialog.Owner = this;
            ItemDialog.ShowDialog();

            if (LastAddedDataIsCorrect == true)
            {
                GeneralBalance.Content = MainUser.Balance;

                HistoryTableEdit(LastAddedData);
                ChartTableEdit(LastAddedData);
                DiagramEdit(LastAddedData);

                DrawCircleDiagram();
            }
        }

        public void AddMoneyToGeneralBalance(double money)
        {
            MainUser.Balance += money;
        }

        private void HistoryTableEdit(History_Data data)
        {
            Random r = new Random();
            if (_dataGrid == null)
            {
                _dataGrid = new List<History_Data>();
                Datagrid.ItemsSource = _dataGrid;
            }
            
            _dataGrid.Add(new History_Data(data.Day, data.Money, data.Category, data.Comment));

            Datagrid.Items.Refresh();
        }

        private void ChartTableEdit(History_Data data)
        {
            if (_dataGridCategories == null)
            {
                _dataGridCategories = new List<Category_Data>();
                DatagridCategory.ItemsSource = _dataGridCategories;
            }
            string category = data.Category;
            int money = Convert.ToInt32(data.Money);

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

        private void DiagramEdit(History_Data data)
        {
            string category = data.Category;
            int money = Convert.ToInt32(data.Money);
            if (_diagramData.ContainsKey(category)) // если категория уже есть
                _diagramData[category] += money; // увеличиваю кол-во денег в ней
            else
                _diagramData.Add(category, money); // добавление новой категории в диаграмму
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


    public class User
    {
        List<History_Data> DATAGrid;

        List<Category> CategoryList;

        public User()
        {
            DATAGrid = new List<History_Data>();
            LoadCategory();
            CardHistory();
        }

        public List<History_Data> Data
        {
            set { DATAGrid = value; }
            get { return DATAGrid; }
        }

        private void CardHistory()
        {

        }


        private void LoadCategory()
        {

        }

        public double Balance { get; set; }

        public void AddItem (History_Data NewItem) => DATAGrid.Add(NewItem);

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
