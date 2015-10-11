using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSpectralGraph;
using DataMining;
using System.Windows.Media;
using Ultilities;

namespace Algorithm
{
    class GraphBase : Draw
    {
        #region Variable

        private Dataset _data;
        private double _xichma;
        private int _k;
        
        #endregion

        #region Properties

        public Dataset Data
        {
            get
            {
                return _data;
            }
            set
            {
                if (value != _data)
                    _data = value;
            }
        }


        public int k
        {
            get
            {
                return this._k;
            }
            set
            {
                if (value != _k)
                    this._k = value;
            }
        }

        public double Xichma
        {
            get
            {
                return this._xichma;
            }
            set
            {
                if (value != _xichma)
                    this._xichma = value;
            }
        }


        #endregion

        #region Method

        /// <summary>
        /// Default constructor
        /// </summary>
        public GraphBase()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Dataset here</param>
        public GraphBase(Dataset data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Dataset here</param>
        /// <param name="xichma">xichma in wij = exp( ||xi-xj||^2 / 2*xichma^2 )</param>
        /// <param name="k">kNN of xi</param>
        public GraphBase(Dataset data, double xichma, int k)
        {
            this.Data = data;
            this.Xichma = xichma;
            this.k = k;
        }

        /// <summary>
        /// Euclidean distance of DPoint dp1 and dp2
        /// </summary>
        /// <param name="dp1">DPoint 1</param>
        /// <param name="dp2">DPoint 2</param>
        /// <returns>distance is Positive if dp1 and dp2 have same label. Distance is Negative if dp1 and dp2 have different label  </returns>
        public static double Distance(DPoint dp1, DPoint dp2)
        {
            double x1 = dp1.MyNode[0].Value;
            double y1 = dp1.MyNode[1].Value;
            double x2 = dp2.MyNode[0].Value;
            double y2 = dp2.MyNode[1].Value;

            if (dp1.Label * dp2.Label > 0) //Same label
                return (Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
            else //Different label
                return (-1) * (Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
        }

        /// <summary>
        /// Compute Weight of 2 DPoint dp1 and dp2
        /// </summary>
        /// <param name="dp1">DPoint 1</param>
        /// <param name="dp2">DPoint 2</param>
        /// <returns>weight of 2 vertex dp1 and dp2</returns>
        public double Weight(DPoint dp1, DPoint dp2)
        {
            return ( Math.Exp( (-1) * Math.Pow(Math.Abs(Distance(dp1,dp2)),2) / ( 2*Xichma*Xichma)) );
        }

        public void DrawDataset()
        {
            List<DPoint> dpLst = Data.Data.ToList();
            Color color = Color.FromArgb(255, 0, 200, 240);

            foreach (var dp in dpLst)
            {
                switch((int)dp.Label)
                {
                    case 0:
                        color = Color.FromArgb(255, 0, 200, 240);
                        break;
                    case 1:
                        color = Color.FromArgb(255, 255, 0, 0);
                        break;
                    case -1:
                        color = Color.FromArgb(255, 0, 230, 0);
                        break;
                }

                Point(8, "(" + dp.MyNode[0].Value + ";" + dp.MyNode[1].Value + ")", dp.MyNode[0].Value, dp.MyNode[1].Value, color);
            }
        }

        public void DrawSG_for_GBS3VDD()
        {
            int countK = 0;
            double[,] dist = new double[Data.Count, Data.Count];

            //Create Distance Matrix (nxn) 
            for (int i = 0; i < Data.Count; ++i)
                for (int j = 0; j < Data.Count; ++j)
                    dist[i, j] = dist[j, i] = Distance(Data.Data[i], Data.Data[j]);

            //Find kNN
            for (int i = 0; i < Data.Count; ++i)
            {
                List<double> tmp = new List<double>();
                int minIndex;
                countK = 0;

                for (int j = 0; j < Data.Count; ++j)
                {
                    if (dist[i, j] == 0) tmp.Add(double.MaxValue);
                    if (dist[i, j] < 0) tmp.Add(Math.Abs(dist[i, j]));
                    if (dist[i, j] > 0) tmp.Add(dist[i, j]);
                }

                do
                {
                    countK++;
                    minIndex = tmp.IndexOf(tmp.Min()); //Get index of tmp List
                    tmp[minIndex] = double.MaxValue; //Set min = MaxValue

                    //Draw line
                    DPoint dp1 = Data.Data[i];
                    DPoint dp2 = Data.Data[minIndex];

                    if (dp1.Label != 0) //dp1 is labeled
                    {
                        if (dp2.Label == 0)
                        {
                            switch ((int)dp1.Label)
                            {
                                case 1:
                                    color = Brushes.OrangeRed;
                                    break;
                                case -1:
                                    color = Brushes.DarkGreen;
                                    break;
                            }

                            Line(dp1.MyNode[0].Value, dp1.MyNode[1].Value, dp2.MyNode[0].Value, dp2.MyNode[1].Value, 2);
                        }
                    }
                    else //dp1 is unlabeled
                    {
                        switch ((int)dp2.Label)
                        {
                            case 0:
                                color = Brushes.DarkCyan;
                                break;
                            case 1:
                                color = Brushes.OrangeRed;
                                break;
                            case -1:
                                color = Brushes.DarkGreen;
                                break;
                        }

                        Line(dp1.MyNode[0].Value, dp1.MyNode[1].Value, dp2.MyNode[0].Value, dp2.MyNode[1].Value, 2);
                    }
                } while (countK != k);

            }
        }

        public void DrawVSG()
        {
            int countK = 0;
            double[,] dist = new double[Data.Count, Data.Count];

            //Create Distance Matrix (nxn) 
            for (int i = 0 ; i < Data.Count; ++i)
                for (int j = 0; j < Data.Count; ++j)
                    dist[i, j] = dist[j,i] = Distance(Data.Data[i], Data.Data[j]);

            //Find kNN
            for (int i =0; i<Data.Count; ++i)
            {
                List<double> tmp = new List<double>();
                int minIndex;
                countK = 0;

                for (int j =0; j < Data.Count ;++j)
                {
                    if (dist[i, j] == 0) tmp.Add(double.MaxValue);
                    if (dist[i, j] < 0) tmp.Add(Math.Abs(dist[i, j]));
                    if (dist[i, j] > 0) tmp.Add(dist[i, j]);
                }
                
                do
                {
                    countK++;
                    minIndex = tmp.IndexOf(tmp.Min()); //Get index of tmp List
                    tmp[minIndex] = double.MaxValue; //Set min = MaxValue

                    //Draw line
                    DPoint dp1 = Data.Data[i];
                    DPoint dp2 = Data.Data[minIndex];

                    if (dp1.Label == dp2.Label)
                    {
                        switch ((int)dp1.Label)
                        {
                            case 0:
                                color = Brushes.DarkCyan;
                                break;
                            case 1:
                                color = Brushes.OrangeRed;
                                break;
                            case -1:
                                color = Brushes.DarkGreen;
                                break;
                        }
                    }
                    else
                    {
                        if ((dp1.Label == 1 && dp1.Label == -1) || (dp1.Label == -1 && dp2.Label == 1)) color = Brushes.DarkViolet;
                        if ((dp1.Label == 0 && dp1.Label == -1) || (dp1.Label == -1 && dp2.Label == 0)) color = Brushes.DarkGreen;
                        if ((dp1.Label == 0 && dp1.Label == 1) || (dp1.Label == 1 && dp2.Label == 0)) color = Brushes.OrangeRed;
                    }

                    Line(dp1.MyNode[0].Value, dp1.MyNode[1].Value, dp2.MyNode[0].Value, dp2.MyNode[1].Value, 2);

                } while (countK != k);

            }
        }

        #endregion
    }
}
