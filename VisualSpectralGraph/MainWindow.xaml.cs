using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Algorithm;
using DataMining;
using Ultilities;

namespace VisualSpectralGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variable

        GraphBase graph;
        Clustering clustering;
        Draw draw;
        Dataset dataset;
        public Dataset restrictDataset;
        public int lbPos, lbNeg;
        private string datasetName;
        List<DPoint> lstPoint;

        Color pColor; //Point color
        private Color rColor; //Rectangle color
        sbyte pSign; //Point sign
        Draw.PointType pType; //Point type

        bool isMouseDown;
        Point mouseDownPoint;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            
            draw = new Draw(drawingPad);
            dataset = new Dataset();
            restrictDataset = new Dataset();
            lstPoint = new List<DPoint>();
            
            //Initialize
            chkRed.IsChecked = true;
            chkSingle.IsChecked = true;
            pColor = Color.FromArgb(255, 255, 0, 0);
            rColor = Color.FromArgb(70, 0, 150,150);
            pType = Draw.PointType.Single;
            pSign = 1;
            isMouseDown = false;
            cbFunction.Items.Add("Spectral Graph for GB-S3VDD");
            cbFunction.Items.Add("Spectral Graph");
            cbFunction.Items.Add("Clustering");
            cbFunction.SelectedIndex = 0;
        }

        #region Toolbar

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            drawingPad.Children.Clear();
            lstPoint.Clear();
        }

        private void btnSaveData_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            dataset = new Dataset(lstPoint.ToArray());

            saveDialog.DefaultExt = ".txt";
            saveDialog.Filter = "Text documents (.txt) | *.txt";
            Nullable<bool> result = saveDialog.ShowDialog();

            if (result == true)
            {
                dataset.Save(saveDialog.FileName);
                MessageBox.Show("Save dataset completed !", "VSG", MessageBoxButton.OK, MessageBoxImage.Information);
                tbStatus.Text = "Save dataset completed !";
            }
        }

        private void btnOpenData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                drawingPad.Children.Clear();
                lstPoint.Clear();
                dataset = null;
                OpenFileDialog openDlg = new OpenFileDialog();
                openDlg.ShowDialog();
                dataset = Dataset.Read(openDlg.FileName);
                datasetName = openDlg.FileName;

                switch (cbFunction.SelectedItem.ToString())
                {
                    case "Spectral Graph for GB-S3VDD":
                    case "Spectral Graph":
                    case "Clustering":
                        lstPoint = dataset.Data.ToList();
                        graph = new GraphBase(dataset, double.Parse(txtXichma.Text), int.Parse(txtK.Text));
                        graph.myCanvas = drawingPad;
                        graph.DrawDataset();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void chkRed_Click(object sender, RoutedEventArgs e)
        {
            chkGreen.IsChecked = false;
            chkBlue.IsChecked = false;
            pColor = Color.FromArgb(255, 255, 0, 0);
            pSign = 1;
        }

        private void chkGreen_Click(object sender, RoutedEventArgs e)
        {
            chkRed.IsChecked = false;
            chkBlue.IsChecked = false;
            pColor = Color.FromArgb(255, 0, 230, 0);
            pSign = -1;
        }

        private void chkBlue_Click(object sender, RoutedEventArgs e)
        {
            chkRed.IsChecked = false;
            chkGreen.IsChecked = false;
            pColor = Color.FromArgb(255, 0, 200, 240);
            pSign = 0;
        }
        
        private void chkSingle_Click(object sender, RoutedEventArgs e)
        {
            chkSparse.IsChecked = false;
            chkDense.IsChecked = false;
            pType = Draw.PointType.Single;
        }

        private void chkSparse_Click(object sender, RoutedEventArgs e)
        {
            chkSingle.IsChecked = false;
            chkDense.IsChecked = false;
            pType = Draw.PointType.Sparse;
        }

        private void chkDense_Click(object sender, RoutedEventArgs e)
        {
            chkSparse.IsChecked = false;
            chkSingle.IsChecked = false;
            pType = Draw.PointType.Dense;
        }

        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void btnPoint_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void btnSaveImg_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            dataset = new Dataset(lstPoint.ToArray());

            saveDialog.DefaultExt = ".png";
            saveDialog.Filter = "PNG (.png) | *.png";
            Nullable<bool> result = saveDialog.ShowDialog();

            if (result == true)
            {
                string imgPath = saveDialog.FileName;
                FileStream fs = new FileStream(imgPath, FileMode.Create);
                RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)drawingPad.ActualWidth, (int)drawingPad.ActualHeight, 1 / 96, 1 / 96, PixelFormats.Pbgra32);

                renderBitmap.Render(drawingPad);
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(fs);
                fs.Close();

                MessageBox.Show("Capture and Save image completed !", "VSG", MessageBoxButton.OK, MessageBoxImage.Information);
                tbStatus.Text = "Capture and Save image completed !";
            }
        }

        private void btnDraw_Click(object sender, RoutedEventArgs e)
        {
            switch (cbFunction.SelectedItem.ToString())
            {
                case "Spectral Graph for GB-S3VDD": //Draw Visual Spectral Graph for GB-S3VDD
                    //Clear old Edge
                    for (int i = 0; i < drawingPad.Children.Count; ++i)
                    {
                        string obj = drawingPad.Children[i].ToString();

                        if (obj.IndexOf("Line") != -1)
                        {
                            drawingPad.Children.RemoveAt(i);
                            i--;
                        }
                    }

                    dataset = new Dataset(lstPoint.ToArray());

                    graph = new GraphBase(dataset, double.Parse(txtXichma.Text), int.Parse(txtK.Text));
                    graph.myCanvas = drawingPad;
                    graph.DrawSG_for_GBS3VDD();

                    tbStatus.Text = "Visual Spectral Graph for GB-S3VDD (with k = " + txtK.Text + " & σ = " + txtXichma.Text + ") completed !";
                    break;
                case "Spectral Graph": //Draw Visual spectral graph
                    //Clear old Edge
                    for (int i = 0; i < drawingPad.Children.Count; ++i)
                    {
                        string obj = drawingPad.Children[i].ToString();

                        if (obj.IndexOf("Line") != -1)
                        {
                            drawingPad.Children.RemoveAt(i);
                            i--;
                        }
                    }

                    dataset = new Dataset(lstPoint.ToArray());

                    graph = new GraphBase(dataset, double.Parse(txtXichma.Text), int.Parse(txtK.Text));
                    graph.myCanvas = drawingPad;
                    graph.DrawVSG();

                    tbStatus.Text = "Visual Spectral Graph (with k = " + txtK.Text + " & σ = " + txtXichma.Text + ") completed !";
                    break;
            }
        }

        private void btnClearEdge_Click(object sender, RoutedEventArgs e)
        {
            //Clear old Edge
            for (int i = 0; i < drawingPad.Children.Count; ++i)
            {
                string obj = drawingPad.Children[i].ToString();

                if (obj.IndexOf("Line") != -1)
                {
                    drawingPad.Children.RemoveAt(i);
                    i--;
                }
            }

        }

        #endregion

        #region Drawing pad

        private void drawingPad_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(drawingPad);

            switch (pType)
            {
                case Draw.PointType.Single:
                    draw.Point(8, "(" + p.X + ";" + p.Y + ")", p.X, p.Y, pColor, pSign, lstPoint);
                    break;
                case Draw.PointType.Sparse:
                    draw.MultiPoint(70, 20, p.X, p.Y, pColor, pSign, lstPoint);
                    break;
                case Draw.PointType.Dense:
                    draw.MultiPoint(40, 25, p.X, p.Y, pColor, pSign, lstPoint);
                    break;
            }
        }

        private void drawingPad_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(drawingPad);
            tbPosition.Text = "(" + p.X + ";" + p.Y + ")";
            
            //if (isMouseDown) draw.Rectangle("", mouseDownPoint.X, mouseDownPoint.Y, Math.Abs(p.X - mouseDownPoint.X), Math.Abs(p.Y - mouseDownPoint.Y), 2, rColor);
        }

        private void drawingPad_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            mouseDownPoint = new Point(e.GetPosition(drawingPad).X, e.GetPosition(drawingPad).Y);
        }

        private void drawingPad_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(drawingPad).X;
            double y = e.GetPosition(drawingPad).Y;
            Point mouseUpPoint = new Point(x, y);

            if (x < mouseDownPoint.X) SwapPoint(ref mouseDownPoint,ref mouseUpPoint);


            rColor = Color.FromArgb(70, 0, 150, 150);
            draw.Rectangle("SelectRectangle", mouseDownPoint.X, mouseDownPoint.Y, Math.Abs(mouseUpPoint.X - mouseDownPoint.X), Math.Abs(mouseUpPoint.Y - mouseDownPoint.Y), 2, rColor);
            isMouseDown = false;
        }

        #endregion

        #region Method

        public void SwapPoint(ref Point pt1,ref Point pt2)
        {
            Point tmp = new Point();

            tmp = pt1;
            pt1 = pt2;
            pt2 = tmp;
        }

        #endregion





    }
}
