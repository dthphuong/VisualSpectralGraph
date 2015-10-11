using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using VisualSpectralGraph;
using DataMining;


namespace Ultilities
{
    public class Draw
    {
        public enum PointType
        {
            Single,
            Sparse,
            Dense,
        }

        public Canvas myCanvas { get; set; }
        //public List<DataItem> data { get; set; }
        public int xD { get; set; }
        public int yD { get; set; }



        #region Adjust

        int fontSize = 12;
        public Brush color = Brushes.Black;


        #endregion

        #region Method

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Draw()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="myCanvas">your Canvas here</param>
        public Draw(Canvas myCanvas)
        {
            this.myCanvas = myCanvas;
        }

        /// <summary>
        /// Draw new Line into myCanvas
        /// </summary>
        /// <param name="X1">x1</param>
        /// <param name="Y1">y1</param>
        /// <param name="X2"x2></param>
        /// <param name="Y2">y2</param>
        /// <param name="strokeThickness">thickness</param>
        /// <param name="color">color</param>
        public void Line(double X1, double Y1, double X2, double Y2, int strokeThickness)
        {
            Line l = new Line();

            l.X1 = X1;
            l.Y1 = Y1;
            l.X2 = X2;
            l.Y2 = Y2;
            l.StrokeThickness = strokeThickness;
            l.Stroke = color;

            myCanvas.Children.Add(l);
        }

        /// <summary>
        /// Draw new Text string into myCanvas
        /// </summary>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <param name="text">text String</param>
        /// <param name="color">Color of text</param>
        public void Text(double X, double Y, string text)
        {
            TextBlock txt = new TextBlock();

            txt.FontSize = fontSize;
            txt.TextAlignment = System.Windows.TextAlignment.Center;
            txt.Text = text;
            txt.Margin = new System.Windows.Thickness(X - txt.Text.Length, Y, 0, 0);
            txt.Foreground = color;

            myCanvas.Children.Add(txt);
        }

        /// <summary>
        /// Draw new Rectangle into myCanvas
        /// </summary>
        /// <param name="toolTip">tooltip of this Rectangle</param>
        /// <param name="X">X postion</param>
        /// <param name="Y">Y position</param>
        /// <param name="width">width of Rectangle</param>
        /// <param name="height"height of Rectangle></param>
        /// <param name="strokeThickness">Border thickness</param>
        /// <param name="color">color of Rectangle</param>
        public void Rectangle(string toolTip, double X, double Y, double width, double height, int strokeThickness, Color color)
        {
            Rectangle r = new Rectangle();
            SolidColorBrush solidColor = new SolidColorBrush(color);

            r.Margin = new System.Windows.Thickness(X, Y, 0, 0);
            r.Width = width;
            r.Height = height;
            r.StrokeThickness = strokeThickness;
            //r.Stroke = solidColor;
            r.Fill = solidColor;
            r.ToolTip = toolTip;

            myCanvas.Children.Add(r);
        }

        /// <summary>
        /// Add new Point into my Canvas
        /// </summary>
        /// <param name="r">the Point radius</param>
        /// <param name="toolTip">Tooltip of Point</param>
        /// <param name="X">coordinates X</param>
        /// <param name="Y">coordinates Y</param>
        /// <param name="color">color of the Point</param>
        public void Point(double r, string toolTip, double X, double Y, Color color)
        {
            Ellipse e = new Ellipse();
            SolidColorBrush soliColor = new SolidColorBrush(color);

            e.Margin = new System.Windows.Thickness(X - r / 2, Y - r / 2, 0, 0);
            e.Width = e.Height = r;
            e.StrokeThickness = 0.7;
            e.Stroke = Brushes.Gray;
            e.Fill = soliColor;
            e.ToolTip = toolTip;

            myCanvas.Children.Add(e);
        }

        /// <summary>
        /// Add new Point into my Canvas
        /// </summary>
        /// <param name="r">the Point radius</param>
        /// <param name="toolTip">Tooltip of Point</param>
        /// <param name="X">coordinates X</param>
        /// <param name="Y">coordinates Y</param>
        /// <param name="color">color of the Point</param>
        /// <param name="pSign">the sign of the Point</param>
        /// <param name="lstPoint">List to save add point in Canvas to List</param>
        public void Point(double r, string toolTip, double X, double Y, Color color, int pSign, List<DPoint> lstPoint)
        {
            Ellipse e = new Ellipse();
            SolidColorBrush soliColor = new SolidColorBrush(color);

            e.Margin = new System.Windows.Thickness(X - r / 2, Y - r / 2, 0, 0);
            e.Width = e.Height = r;
            e.StrokeThickness = 0.7;
            e.Stroke = Brushes.Gray;
            e.Fill = soliColor;
            e.ToolTip = toolTip;

            myCanvas.Children.Add(e);
            lstPoint.Add(new DPoint(pSign, new List<Node> { new Node(1, X), new Node(2, Y) }));
        }

        /// <summary>
        /// Add Multi Point into my canvas
        /// </summary>
        /// <param name="d">the radius of "point zone"</param>
        /// <param name="n">number of point</param>
        /// <param name="X">coordinates X</param>
        /// <param name="Y">coordinates Y</param>
        /// <param name="color">color of the Point</param>
        /// <param name="pSign">the sign of the Point</param>
        /// <param name="lstPoint">List to save add point in Canvas to List</param>

        public void MultiPoint(double d, int n, double X, double Y, Color color, int pSign, List<DPoint> lstPoint)
        {
            double x = 0, y = 0;
            Random rnd = new Random(DateTime.Now.Millisecond);
            SolidColorBrush solidColor = new SolidColorBrush(color);

            for (int i = 1; i<=n ; ++i)
            {
                x = rnd.Next(Convert.ToInt32(X - d / 2), Convert.ToInt32(X + d / 2));
                y = rnd.Next(Convert.ToInt32(Y - d / 2), Convert.ToInt32(Y + d / 2));
                Point(8, "(" + x + "," + y + ")", x, y, color, pSign, lstPoint);
            }


        }
            


        #endregion


    }
}
