using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NearestPoints
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Points points = new Points();
        private int width, height;
        private List<Ellipse> labeledPoint = new List<Ellipse>();
        public MainWindow()
        {
            InitializeComponent();
            Loaded += delegate
            {
                width = (int)canvas.ActualWidth;
                height = (int)canvas.ActualHeight;
                initAxis();
            };
        }

        private void initAxis()
        {
            while (xAxis.Children.Count > 0) xAxis.Children.RemoveAt(0);
            while (yAxis.Children.Count > 0) yAxis.Children.RemoveAt(0);
            for (int i = 0; i < xAxis.ActualWidth; i += 50)
            {
                TextBlock t = new TextBlock();
                t.Text = i + "";
                Canvas.SetBottom(t, 10);
                Canvas.SetLeft(t, i);
                xAxis.Children.Add(t);
            }
            for (int i = 0; i < yAxis.ActualHeight; i += 50)
            {
                TextBlock t = new TextBlock();
                t.Text = i + "";
                Canvas.SetRight(t, 10);
                Canvas.SetTop(t, i);
                yAxis.Children.Add(t);
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine(e.GetPosition(canvas).X + " " + e.GetPosition(canvas).Y);
            double x = e.GetPosition(canvas).X, y = e.GetPosition(canvas).Y;
            points.addPoint(x, y);
            drawPoint(x, y, Colors.Black);
        }

        private Ellipse drawPoint(double x, double y, Color c)
        {
            Ellipse ellipse = new Ellipse();
            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = c;
            ellipse.Fill = solidColorBrush;
            ellipse.Width = 6;
            ellipse.Height = 6; 
            Canvas.SetTop(ellipse, y);
            Canvas.SetLeft(ellipse, x);
            canvas.Children.Add(ellipse);
            return ellipse;
        }

        private void labelPoint(Point p)
        {
            labeledPoint.Add(drawPoint(p.x, p.y, Colors.Red));
        }

        private void drawPoints()
        {
            int i = 0;
            foreach (Point point in points.arr) {
                if (i == 10000) // 为了性能考虑， 最多只绘制10000个点（多了也看不清）
                    break;
                i++;
                drawPoint(point.x, point.y, Colors.Black);
            }
        }

        private void PointsNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void AddPoints_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.RemoveRange(0, canvas.Children.Count);
            int num = Convert.ToInt32(PointsNum.Text);
            points = new Points(num, width, height);
            drawPoints();
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            if (points.arr.Count == 0)
            {
                result.Content = "没有点，无法计算距离";
                return;
            }
            if (points.arr.Count == 1)
            {
                result.Content = "只有一个点, 无法计算距离";
                return;
            }
            foreach (Ellipse elli in labeledPoint)
            {
                canvas.Children.Remove(elli);
            }
            double ans = points.getSmallestDistant();
            result.Content = "最短距离是：" + ans + " 对应点为红色点";
            labelPoint(points.arr[points.p1]);
            labelPoint(points.arr[points.p2]);
            points.ordinaryMethod();
        }

        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            width = (int)e.NewSize.Width;
            height = (int)e.NewSize.Height;
            initAxis();
        }
    }
}
