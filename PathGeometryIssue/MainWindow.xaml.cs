using System;
using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using WinPoint = Windows.Foundation.Point;

namespace PathGeometryIssue;

public sealed partial class MainWindow : Window
{
  private TranslateTransform _tf;

  public MainWindow()
  {
    this.InitializeComponent();
    LoadData();
  }

  private void LoadData()
  {
    // Canvas.Width isn't valid until loaded() gets called - just cheat for now.
    PointCollection pc = GenerateSampleData(700, 700);

    var pathGeom = new PathGeometry();
    _tf = new TranslateTransform();
    
    // If this is left at 0, then nothing appears on the canvas.
    _tf.X = .01;
    _tf.Y = 0;

    pathGeom.Transform = _tf;

    var pf = new PathFigure();
    pathGeom.Figures.Add(pf);

    var pls = new PolyLineSegment();
    pls.Points = pc;
    pf.StartPoint = pc[0];
    pf.Segments.Add(pls);

    var shp = new Path();
    shp.Data = pathGeom;
    shp.StrokeThickness = 2;
    shp.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));

    cnv.Children.Add(shp);
  }

  private static PointCollection GenerateSampleData(double pxWide, double pxTall)
  {
    var pc = new PointCollection();

    // SinWave sample data
    double left = 0;
    double right = 4 * Math.PI;
    double width = right - left;

    double bot = -1.2;
    double top = 1.2;
    double height = top - bot;

    int divs = 1000;
    for (int i = 0; i < divs; i++)
    {
      double frac = (double)i/divs;

      double x = left + width * frac;
      double y = Math.Sin(x);

      // To pixel space...
      double xPx = frac * pxWide;

      double fracFromTop = (top - y) / height;
      double yPx = fracFromTop * pxTall;

      pc.Add(new WinPoint(xPx, yPx));
    }

    // Simple line displays the same issue
    //pc.Add(new WinPoint(0, 0));
    //pc.Add(new WinPoint(pxWide, pxTall));

    return pc;
  }

  private void ResetTransform(object sender, RoutedEventArgs e)
  {
    _tf.X = 0;
    _tf.Y = 0;
  }

  private void MoveRight(object sender, RoutedEventArgs e)
  {
    _tf.X += 1;
  }

  private void MoveDown(object sender, RoutedEventArgs e)
  {
    _tf.Y += 1;
  }
}
