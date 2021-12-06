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
        public MainWindow()
        {
            InitializeComponent();
            DrawCircleDiagram();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RollUp_Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Expenses_Button_Click(object sender, RoutedEventArgs e)
        {
            Income_Button.Opacity = 0.5;
            Expenses_Button.Opacity = 1;
        }

        private void Income_Button_Click(object sender, RoutedEventArgs e)
        {
            Income_Button.Opacity = 1;
            Expenses_Button.Opacity = 0.5;
        }

        private void DrawCircleDiagram()
        {
            int sum = 360;
            int angles = 5;
            double radius = 100.0;

            var startAngle = 0.0;

            var centerPoint = new Point(radius, radius);
            var xyradius = new Size(radius, radius);

            for (int angle = 0; angle < angles; angle++)
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
                    Fill = Brushes.Red,
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
}
