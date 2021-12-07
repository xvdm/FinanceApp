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
        List<History_Data> data_grid = null;
        TransverSetting Transver = new TransverSetting();

        private bool _expanded = false;

        public MainWindow()
        {
            InitializeComponent();
            this.Height = System.Windows.SystemParameters.WorkArea.Height / 1.2;
            this.Width = System.Windows.SystemParameters.WorkArea.Width / 1.2;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

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
            MessageBox.Show("In coming future", "Setting");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(data_grid == null)
            {
                data_grid = new List<History_Data>();
                Datagrid.ItemsSource = data_grid;
            }
            data_grid.Add(new History_Data("06.12.2021", 600, "Вадим", "Диме"));



            Transver.General_Balance += 600;
            GeneralBalance.Content = Transver.General_Balance;

            Datagrid.Items.Refresh();
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
            int[] data = new int[3] { 100, 200, 300 };
            var sum = data.Sum();
            var angles = data.Select(d => d * 2.0 * Math.PI / sum);

            double radius = 150.0;
            var startAngle = 0.0;

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
                MainBorder.Children.Add(p);

                startAngle = endAngle;
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
        private string day;
        private double money;
        private string category;
        private string comment;

        public History_Data(string _day, double _money, string _category, string _comment)
        {
            this.day = _day;
            this.money = _money;
            this.category = _category;
            this.comment = _comment;
        }
        public string Day { get { return this.day; } set { this.day = value; } }
        public double Money { get { return this.money; } set { this.money = value; } }
        public string Category { get { return this.category; } set { this.category = value; } }
        public string Comment { get { return this.comment; } set { this.comment = value; } }
    }
}
