using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class DrawCanvas
{
    public static void DrawSection(Canvas canvas, double BottomLength, double TopLength, double BottomHeight,
        double TopHeight, double Depth)
    {
        double side = Constants.SIDE;
        double top = Constants.TOP;
        double cWidth = Constants.CANVAS_WIDTH;
        double cHeight = Constants.CANVAS_HEIGHT;
        double height = BottomHeight + TopHeight;
        double scale = GetScale(BottomLength, height, side, top, cWidth, cHeight);
        Point p0 = new Point(cWidth / 2, cHeight / 2);
        #region axes
        Point px1 = new Point(p0.X - cWidth / 2, p0.Y);
        Point px2 = new Point(p0.X + cWidth / 2, p0.Y);
        PolylineWay(canvas, new List<Point>{px1, px2});
        Point py1 = new Point(p0.X, p0.Y - cHeight / 2);
        Point py2 = new Point(p0.X, p0.Y + cHeight / 2);
        PolylineWay(canvas, new List<Point>{py1, py2});
        #endregion
        #region Bottom FDN
        Point f1 = new Point(p0.X - BottomLength / scale / 2, p0.Y + height / scale / 2);
        Point f2 = new Point(f1.X + BottomLength / scale, f1.Y);
        Point f3 = new Point(f2.X, f2.Y - BottomHeight / scale);
        Point f4 = new Point(f1.X, f3.Y);
        SolidColorBrush sc = Brushes.Black;
        CreatePolyline(canvas, new List<Point> { f1, f2, f3, f4, f1 }, 0.75, sc, true);
        #endregion
        #region Top FDN
        Point f5 = new Point(p0.X - TopLength / scale / 2, p0.Y - height / scale / 2);
        Point f6 = new Point(f5.X + TopLength / scale, f5.Y);
        Point f7 = new Point(f6.X, f6.Y + TopHeight / scale);
        Point f8 = new Point(f5.X, f7.Y);
        CreatePolyline(canvas, new List<Point> { f5, f6, f7, f8, f5 }, 0.75, sc, true);
        #endregion
        #region Ground
        Point g1 = new Point(f5.X + 30, f5.Y + Depth / scale);
        Point g2 = new Point(p0.X - TopLength / scale / 2, g1.Y);
        sc = Brushes.Red;
        CreatePolyline(canvas, new List<Point> { g1, g2 }, 0.5, sc, false);
        #endregion

        #region DIM
        Point p1 = new Point(p0.X + BottomLength / scale / 2 + 20, p0.Y +  height / scale / 2);
        Point p2 = new Point(p1.X, p1.Y - height / scale);
        sc = Brushes.Blue;
        CreatePolyline(canvas, new List<Point> { p1, p2 }, 0.75, sc);
        Point e1 = new Point(p1.X - 50 / scale, p1.Y + 50 / scale);
        Point e2 = new Point(p1.X + 50 / scale, p1.Y - 50 / scale);
        CreatePolyline(canvas, new List<Point> { e1, e2 }, 0.75, sc);
        Point n1 = new Point(e1.X, p1.Y);
        Point n2 = new Point(e2.X, p1.Y);
        CreatePolyline(canvas, new List<Point> { n1, n2 }, 0.75, sc);
        Point e3 = new Point(p2.X - 50 / scale, p2.Y + 50 / scale);
        Point e4 = new Point(p2.X + 50 / scale, p2.Y - 50 / scale);
        CreatePolyline(canvas, new List<Point> { e3, e4 }, 0.75, sc);
        Point n3 = new Point(e3.X, p2.Y);
        Point n4 = new Point(e4.X, p2.Y);
        CreatePolyline(canvas, new List<Point> { n3, n4 }, 0.75, sc);

        Point p3 = new Point(p0.X - BottomLength / scale / 2 - 20, p1.Y);
        Point p4 = new Point(p3.X, p3.Y - (height - Depth) / scale);
        CreatePolyline(canvas, new List<Point> { p3, p4 }, 0.75, sc);
        Point e5 = new Point(p3.X - 50 / scale, p3.Y + 50 / scale);
        Point e6 = new Point(p3.X + 50 / scale, p3.Y - 50 / scale);
        CreatePolyline(canvas, new List<Point> { e5, e6 }, 0.75, sc);
        Point n5 = new Point(e5.X, p3.Y);
        Point n6 = new Point(e6.X, p3.Y);
        CreatePolyline(canvas, new List<Point> { n5, n6 }, 0.75, sc);
        Point e7 = new Point(p4.X - 50 / scale, p4.Y + 50 / scale);
        Point e8 = new Point(p4.X + 50 / scale, p4.Y - 50 / scale);
        CreatePolyline(canvas, new List<Point> { e7, e8 }, 0.75, sc);
        Point n7 = new Point(e7.X, p4.Y);
        Point n8 = new Point(e8.X, p4.Y);
        CreatePolyline(canvas, new List<Point> { n7, n8 }, 0.75, sc);
        Point e9 = new Point(p3.X - 50 / scale, (p1.Y - BottomHeight / scale) + 50 / scale);
        Point e10 = new Point(p3.X + 50 / scale, e9.Y - 50 / scale);
        CreatePolyline(canvas, new List<Point> { e9, e10 }, 0.75, sc);
        Point n9 = new Point(e9.X, e10.Y);
        Point n10 = new Point(e10.X, e10.Y);
        CreatePolyline(canvas, new List<Point> { n9, n10 }, 0.75, sc);

        Point x1 = new Point(f1.X, f1.Y + 20);
        Point x2 = new Point(f2.X, f2.Y + 20);
        CreatePolyline(canvas, new List<Point> { x1, x2 }, 0.75, sc);
        e1 = new Point(x1.X - 50 / scale, x1.Y + 50 / scale);
        e2 = new Point(x1.X + 50 / scale, x1.Y - 50 / scale);
        CreatePolyline(canvas, new List<Point> { e1, e2 }, 0.75, sc);
        n1 = new Point(x1.X, e1.Y);
        n2 = new Point(x1.X, e2.Y);
        CreatePolyline(canvas, new List<Point> { n1, n2 }, 0.75, sc);
        e3 = new Point(x2.X - 50 / scale, x2.Y + 50 / scale);
        e4 = new Point(x2.X + 50 / scale, x2.Y - 50 / scale);
        CreatePolyline(canvas, new List<Point> { e3, e4 }, 0.75, sc);
        n3 = new Point(x2.X, e3.Y);
        n4 = new Point(x2.X, e4.Y);
        CreatePolyline(canvas, new List<Point> { n3, n4 }, 0.75, sc);

        Point pMid = MidPoint(p1, p2);
        DimVertical(canvas, pMid, height, false);
        pMid = MidPoint(new Point(p3.X, p3.Y - BottomHeight / scale / 2), new Point(p4.X, p4.Y - BottomHeight / scale / 2));
        DimVertical(canvas, pMid, BottomHeight, true);
        pMid = MidPoint(new Point(p3.X, p3.Y - BottomHeight / scale / 2), new Point(p4.X, e10.Y + 30));
        DimVertical(canvas, pMid,  TopHeight, true);

        pMid = MidPoint(f1, f2);
        DimHorizontal(canvas, pMid, BottomLength);
        #endregion
    }

    public static void DrawPlan(Canvas canvas, double BottomLength, double TopLength, double BottomCenter, 
        double TopCenter, double BCDCircleRadius)
    {
        double side = Constants.SIDE;
        double top = Constants.TOP;
        double cWidth = Constants.CANVAS_WIDTH;
        double cHeight = Constants.CANVAS_HEIGHT;
        double scale = GetScale(BottomLength, TopLength, side, top, cWidth, cHeight);
        Point p0 = new Point(cWidth / 2, cHeight / 2);
        SolidColorBrush sc = Brushes.Red;
        #region axes
        Point px1 = new Point(p0.X - cWidth / 2, p0.Y);
        Point px2 = new Point(p0.X + cWidth / 2, p0.Y);
        PolylineWay(canvas, new List<Point> { px1, px2 });
        Point py1 = new Point(p0.X, p0.Y - cHeight / 2);
        Point py2 = new Point(p0.X, p0.Y + cHeight / 2);
        PolylineWay(canvas, new List<Point> { py1, py2 });
        #endregion
        #region BCD Circle
        double radius = BCDCircleRadius / scale / 2;
        CreateCircle(canvas, p0, radius, 0.75, sc);
        #endregion
        #region BottomFDN
        sc = Brushes.Black;
        Point f1 = new Point(p0.X - BottomCenter / scale / 2, p0.Y - BottomLength / scale / 2);
        Point f2 = new Point(f1.X + BottomCenter / scale, f1.Y);
        Point f3 = new Point(p0.X + BottomLength / scale / 2, p0.Y - BottomCenter / scale / 2);
        Point f4 = new Point(f3.X, f3.Y + BottomCenter / scale);
        Point f5 = new Point(f2.X, p0.Y + BottomLength / scale / 2);
        Point f6 = new Point(f1.X, f5.Y);
        Point f7 = new Point(p0.X - BottomLength / scale / 2, f4.Y);
        Point f8 = new Point(f7.X, f3.Y);
        CreatePolyline(canvas, new List<Point> { f1, f2, f3, f4, f5, f6, f7, f8, f1 }, 0.75, sc, true);
        #endregion
        #region TopFDN
        f1 = new Point(p0.X - TopCenter / scale / 2, p0.Y - TopLength / scale / 2);
        f2 = new Point(f1.X + TopCenter / scale, f1.Y);
        f3 = new Point(p0.X + TopLength / scale / 2, p0.Y - TopCenter / scale / 2);
        f4 = new Point(f3.X, f3.Y + TopCenter / scale);
        f5 = new Point(f2.X, p0.Y + TopLength / scale / 2);
        f6 = new Point(f1.X, f5.Y);
        f7 = new Point(p0.X - TopLength / scale / 2, f4.Y);
        f8 = new Point(f7.X, f3.Y);
        CreatePolyline(canvas, new List<Point> { f1, f2, f3, f4, f5, f6, f7, f8, f1 }, 0.75, sc, true);
        #endregion
        #region DIM
        Point p1 = new Point(p0.X + BottomLength / scale / 2 + 60, p0.Y + BottomLength / scale / 2);
        Point p2 = new Point(p1.X, p1.Y - BottomLength / scale);
        sc = Brushes.Blue;
        CreatePolyline(canvas, new List<Point> { p1, p2 }, 0.75, sc);
        Point e1 = new Point(p1.X - 50 / scale, p1.Y + 50 / scale);
        Point e2 = new Point(p1.X + 50 / scale, p1.Y - 50 / scale);
        CreatePolyline(canvas, new List<Point> { e1, e2 }, 0.75, sc);
        Point n1 = new Point(e1.X, p1.Y);
        Point n2 = new Point(e2.X, p1.Y);
        CreatePolyline(canvas, new List<Point> { n1, n2 }, 0.75, sc);
        Point e3 = new Point(p2.X - 50 / scale, p2.Y + 50 / scale);
        Point e4 = new Point(p2.X + 50 / scale, p2.Y - 50 / scale);
        CreatePolyline(canvas, new List<Point> { e3, e4 }, 0.75, sc);
        Point n3 = new Point(e3.X, p2.Y);
        Point n4 = new Point(e4.X, p2.Y);
        CreatePolyline(canvas, new List<Point> { n3, n4 }, 0.75, sc);
        Point pMid = MidPoint(p1, p2);
        DimVertical(canvas, pMid, BottomLength, false);

        Point p3 = new Point(p0.X + BottomLength / scale / 2 + 20, p0.Y + BottomCenter / scale / 2);
        Point p4 = new Point(p3.X, p3.Y - BottomCenter / scale);
        CreatePolyline(canvas, new List<Point> { p3, p4 }, 0.75, sc);
        e1 = new Point(p3.X - 50 / scale, p3.Y + 50 / scale);
        e2 = new Point(p3.X + 50 / scale, p3.Y - 50 / scale);
        CreatePolyline(canvas, new List<Point> { e1, e2 }, 0.75, sc);
        n1 = new Point(e1.X, p3.Y);
        n2 = new Point(e2.X, p3.Y);
        CreatePolyline(canvas, new List<Point> { n1, n2 }, 0.75, sc);
        e3 = new Point(p4.X - 50 / scale, p4.Y + 50 / scale);
        e4 = new Point(p4.X + 50 / scale, p4.Y - 50 / scale);
        CreatePolyline(canvas, new List<Point> { e3, e4 }, 0.75, sc);
        n3 = new Point(e3.X, p4.Y);
        n4 = new Point(e4.X, p4.Y);
        CreatePolyline(canvas, new List<Point> { n3, n4 }, 0.75, sc);
        pMid = MidPoint(p3, p4);
        DimVertical(canvas, pMid, BottomCenter, false);

        p1 = new Point(p0.X - TopLength / scale / 2, p1.Y + 40);
        p2 = new Point(p1.X + TopLength / scale, p1.Y);
        CreatePolyline(canvas, new List<Point> { p1, p2 }, 0.75, sc);
        e1 = new Point(p1.X - 50 / scale, p1.Y + 50 / scale);
        e2 = new Point(p1.X + 50 / scale, p1.Y - 50 / scale);
        CreatePolyline(canvas, new List<Point> { e1, e2 }, 0.75, sc);
        n1 = new Point(p1.X, e1.Y);
        n2 = new Point(p1.X, e2.Y);
        CreatePolyline(canvas, new List<Point> { n1, n2 }, 0.75, sc);
        e3 = new Point(p2.X - 50 / scale, p2.Y + 50 / scale);
        e4 = new Point(p2.X + 50 / scale, p2.Y - 50 / scale);
        CreatePolyline(canvas, new List<Point> { e3, e4 }, 0.75, sc);
        n3 = new Point(p2.X, e3.Y);
        n4 = new Point(p2.X, e4.Y);
        CreatePolyline(canvas, new List<Point> { n3, n4 }, 0.75, sc);
        pMid = MidPoint(new Point(p1.X, p1.Y - 20), new Point(p2.X, p2.Y - 20));
        DimHorizontal(canvas, pMid, TopLength);

        p1 = new Point(p0.X - TopCenter / scale / 2, p1.Y - 20);
        p2 = new Point(p1.X + TopCenter / scale, p1.Y);
        CreatePolyline(canvas, new List<Point> { p1, p2 }, 0.75, sc);
        e1 = new Point(p1.X - 50 / scale, p1.Y + 50 / scale);
        e2 = new Point(p1.X + 50 / scale, p1.Y - 50 / scale);
        CreatePolyline(canvas, new List<Point> { e1, e2 }, 0.75, sc);
        n1 = new Point(p1.X, e1.Y);
        n2 = new Point(p1.X, e2.Y);
        CreatePolyline(canvas, new List<Point> { n1, n2 }, 0.75, sc);
        e3 = new Point(p2.X - 50 / scale, p2.Y + 50 / scale);
        e4 = new Point(p2.X + 50 / scale, p2.Y - 50 / scale);
        CreatePolyline(canvas, new List<Point> { e3, e4 }, 0.75, sc);
        n3 = new Point(p2.X, e3.Y);
        n4 = new Point(p2.X, e4.Y);
        CreatePolyline(canvas, new List<Point> { n3, n4 }, 0.75, sc);
        pMid = MidPoint(new Point(p1.X, p1.Y - 20), new Point(p2.X, p2.Y - 20));
        DimHorizontal(canvas, pMid, TopCenter);
        #endregion
    }
    public static Point MidPoint(Point p1, Point p2)
    {
        return new Point(p1.X / 2 + p2.X / 2, p1.Y / 2 + p2.Y / 2);
    }

    private static void PolylineWay(Canvas canvas, List<Point> dsP)
    {
        Polyline axes = new Polyline();
        for(int i = 0; i < dsP.Count; i++)
        {
            axes.Points.Add(dsP[i]);
        }
        axes.Stroke = Brushes.Blue;
        DoubleCollection dashes = new DoubleCollection { 4, 8 };
        axes.StrokeDashArray = dashes;
        axes.StrokeThickness = 0.5;
        canvas.Children.Add(axes);
    }
    public static void CreatePolyline(Canvas canvas, List<Point> dsP, double strokeThickness, SolidColorBrush color)
    {
        Polygon polygon = new Polygon();
        for (int i = 0; i < dsP.Count; i++)
        {
            polygon.Points.Add(dsP[i]);
        }
        polygon.StrokeThickness = strokeThickness;
        polygon.Stroke = color;
        canvas.Children.Add(polygon);
    }

    public static void CreatePolyline(Canvas canvas, List<Point> dsP, double strokeThickness, 
        SolidColorBrush color, bool close)
    {
        Polygon polygon = new Polygon();
        for (int i = 0; i < dsP.Count; i++)
        {
            polygon.Points.Add(dsP[i]);
        }
        polygon.StrokeThickness = strokeThickness;
        polygon.Stroke = color;
        if (close)
        {
            List<Point> points = new List<Point>() { dsP[0], dsP[dsP.Count - 1] };
            CreatePolyline(canvas, points, strokeThickness, color);
        }
        canvas.Children.Add(polygon);
    }
    public static void CreateCircle(Canvas canvas, Point center, double radius, double strokeThickness, SolidColorBrush color)
    {
        Ellipse c = new Ellipse();
        c.Width = radius*2;
        c.Height = radius*2;
        c.StrokeThickness = strokeThickness;
        c.Stroke = color;

        Canvas.SetLeft(c, center.X - radius);
        Canvas.SetTop(c, center.Y - radius);
        canvas.Children.Add(c);
    }
    private static double GetScale(double width, double height, double side, double top, 
        double canvasWidth, double canvasHeight)
    {
        double scaleWidth;
        if (width > canvasWidth - 2 * side) scaleWidth = width * 1.2 / (canvasWidth - 2 * side);
        else scaleWidth = 1;
        double scaleHeight;
        if (height > canvasHeight - 2 * top) scaleHeight = height * 1.2 / (canvasHeight - 2 * top);
        else scaleHeight = 1;
        return Math.Max(scaleWidth, scaleHeight);
    }
    public static void DimVertical(Canvas canvas, Point pIn,double length, bool left)
    {
        TextBlock text = new TextBlock();
        text.Text = length.ToString();
        text.FontSize = 10;
        text.FontFamily = new FontFamily("Arial");
        text.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
        text.Arrange(new Rect(text.DesiredSize));
        Canvas.SetTop(text, pIn.Y - text.ActualHeight);
        if(left) Canvas.SetLeft(text, pIn.X - text.ActualWidth - 11);
        else Canvas.SetLeft(text, pIn.X + text.ActualWidth - 10);
        canvas.Children.Add(text);
    }

    public static void DimHorizontal(Canvas canvas, Point pIn, double length)
    {
        TextBlock text = new TextBlock();
        text.Text = length.ToString();
        text.FontSize = 10;
        text.FontFamily = new FontFamily("Arial");
        text.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
        text.Arrange(new Rect(text.DesiredSize));
        Canvas.SetTop(text, pIn.Y - text.ActualHeight + 17);
        Canvas.SetLeft(text, pIn.X - text.ActualWidth + 10);
        canvas.Children.Add(text);
    }
}
