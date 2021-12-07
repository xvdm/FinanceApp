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
    public partial class MainWindow : Window
    {
        private List<History_Data> data_grid = null;

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
            if (data_grid == null)
            {
                data_grid = new List<History_Data>();
                Datagrid.ItemsSource = data_grid;
            }
            Random r = new Random();
            int money = r.Next(100, 2000);
            data_grid.Add(new History_Data("06.12.2021", money, "Вадим", "Диме"));

            Transver.General_Balance += money;
            GeneralBalance.Content = Transver.General_Balance;

            Datagrid.Items.Refresh();

            DrawCircleDiagram();
        }

        private void Сurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Currency.SelectedItem .ToString() == "$")
            {
                double temp = 0;
                temp = Transver.General_Balance / 27.37;
                GeneralBalance.Content = temp;
            }
        }

        private void Button_Click_Expand(object sender, RoutedEventArgs e)
        {
            if (_expanded == true)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;

            _expanded = !_expanded;
        }

        private void DrawCircleDiagram()
        {
            if (data_grid.Count > 1)
            {
                int[] data = new int[data_grid.Count];
                for (int i = 0; i < data_grid.Count; i++)
                {
                    data[i] = Convert.ToInt32(data_grid[i].Money);
                }
                var sum = data.Sum();
                var angles = data.Select(d => d * 2.0 * Math.PI / sum);

                double radius = DiagramCanvas.Width / 2;
                double startAngle = 0.0;

                var centerPoint = new Point(radius, radius);
                var xyradius = new Size(radius, radius);

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
                        Fill = Brushes.Gray,
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
                ellipse.Fill = Brushes.Gray;
                ellipse.Stroke = Brushes.Black;
                ellipse.StrokeThickness = 1;
                DiagramCanvas.Children.Add(ellipse);
            }
        }
    }


    public class TransverSetting
    {
        private double MONEY = 0;

        public double General_Balance { get { return MONEY; } set { MONEY = value; } }
    }


    public class History_Data
    {
        private string _day;
        private double _money;
        private string _category;
        private string _comment;

        public History_Data(string day, double money, string category, string comment)
        {
            _day = day;
            _money = money;
            _category = category;
            _comment = comment;
        }

        public string Day { get { return _day; } set { _day = value; } }
        public double Money { get { return _money; } set { _money = value; } }
        public string Category { get { return _category; } set { _category = value; } }
        public string Comment { get { return _comment; } set { _comment = value; } }
    }
}
