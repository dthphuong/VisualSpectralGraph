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
    class Clustering:Draw
    {
        #region Variable
        private Dictionary<int, DPoint[]> myClusters;
        private Dataset _data;
        private int nrSampling = 100;
        private int kMax = 0;
        private int alpha = 0;

        #endregion

        #region Properties

        public int nrCluster
        {
            get { return myClusters.Count; }
        }

        public Dataset Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public int NrSampling
        {
            get { return nrSampling; }
            set { nrSampling = value; }
        }

        public int KMax
        {
            get { return kMax; }
        }

        public int Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Clustering()
        {
            myClusters = new Dictionary<int, DPoint[]>();
        }

        /// <summary>
        /// Constructor 1
        /// </summary>
        /// <param name="_data">Dataset here</param>
        public Clustering(Dataset _data)
        {
            this._data = _data;
            myClusters = new Dictionary<int, DPoint[]>();
        }

        /// <summary>
        /// Constructor 2
        /// </summary>
        /// <param name="_data">Dataset here</param>
        /// <param name="nrSampling">Number of Sampling</param>
        public Clustering(Dataset _data, int nrSampling)
        {
            this._data = _data;
            this.nrSampling = nrSampling;
        }

        #endregion


        #region Method

        public double Log2(double x)
        {
            return Math.Log10(x)/Math.Log10(2);
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

            return(Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
        }

        /// <summary>
        /// Sampling data by Random 
        /// </summary>
        /// <returns>index array of Cluster</returns>
        public int[] samplingData(int kMax)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            int[] cIndex = new int[_data.Count];

            //Random cIndex array
            for (int i = 0; i < cIndex.Length; ++i)
                cIndex[i] = rnd.Next(1, kMax+1);

            return cIndex;
        }

        /// <summary>
        /// Explore all Cluster of Data
        /// </summary>
        public void exploreCluster()
        {
            int N = _data.Count;
            kMax = alpha * (int) Log2(N);
            double distortionMin = double.MaxValue;
            
            //Explore Cluster by Sampling technique
            for (int t = 1; t <= nrSampling; ++t)
            {
                int[] cIndex = new int[N];
                Dictionary<int, DPoint[]> cluster = new Dictionary<int, DPoint[]>();
                List<double> distortion = new List<double>(); //distortion for nrSampling
                cIndex = samplingData(kMax);

                //Create cluster and calculate Distortion
                for (int i = 1; i <= cIndex.Max(); ++i)
                {
                    List<DPoint> temp = new List<DPoint>();
                    DPoint avg = new DPoint();
                    double avgDistortion = 0;

                    //Create cluster
                    for (int j = 0; j < N; ++j)
                        if (cIndex[j] == i)
                            temp.Add(_data.Data[j]);
                    cluster.Add(i, temp.ToArray());

                    if (temp.Count == 0) continue;

                    //Calculate Distortion
                    avg = temp[0]/temp.Count;
                    for (int j = 1; j < temp.Count; ++j) 
                        avg = avg + temp[j]/temp.Count;
                    
                    //Calculate avgDistortion
                    foreach (DPoint dp in temp)
                        avgDistortion += Distance(dp, avg);

                    distortion.Add(avgDistortion);
                }

                double sum = distortion.Sum();
                if ( sum < distortionMin)
                {
                    distortionMin = sum;
                    myClusters = cluster;
                }
            }
        }

        public void DrawClustering()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);

            foreach (var item in myClusters)
            {
                DPoint[] cluster = item.Value;
                Color pColor = Color.FromArgb(255, (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255));

                foreach (var dpt in cluster)
                {
                    Point(8, "", dpt.MyNode[0].Value, dpt.MyNode[1].Value, pColor );
                }
            }
        }

        #endregion
    }
}
