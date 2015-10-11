using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DataMining;

namespace DataMining
{
    public class Dataset
    {

        #region Variable

        private DPoint[] _data;
        private int _count;
        private int _pos;
        private int _neg;

        #endregion

        #region Properties

        public DPoint[] Data
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

        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                if (value != _count)
                    _count = value;
            }
        }

        public int NrPos
        {
            get
            {
                return _pos;
            }
            set
            {
                if (value != _pos)
                    _pos = value;
            }
        }

        public int NrNeg
        {
            get
            {
                return _neg;
            }
            set
            {
                if (value != _neg)
                    _neg = value;
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Dataset()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_data">your data here</param>
        public Dataset(DPoint[] _data)
        {
            Data = _data;
            Count = Data.Length;
        }

        /// <summary>
        /// Counting number of Positive & Negative point
        /// </summary>
        public void CountPosNeg()
        {
            int nrPos = 0, nrNeg = 0;

            foreach (DPoint dp in Data)
            {
                if (dp.Label > 0)
                    nrPos++;
                else
                    nrNeg++;
            }

            NrPos = nrPos; NrNeg = nrNeg;
        }

        /// <summary>
        /// Save dataset to file
        /// </summary>
        /// <param name="Filename"></param>
        public void Save(string Filename)
        {
            StreamWriter writer = new StreamWriter(Filename);

            for (int i = 0; i < Count; ++i)
            {
                writer.Write(Data[i].Label + " ");
                foreach (var x in Data[i].MyNode) writer.Write(x.Index + ":" + x.Value + " ");
                writer.WriteLine();
            }
            
            writer.Close();
        }

        /// <summary>
        /// Read a Dataset from a Stream
        /// </summary>
        /// <param name="stream">Stream to Read</param>
        /// <returns>The Dataset</returns>
        private static Dataset Read(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            //LibSVM Variable
            List<DPoint> data = new List<DPoint>();
            int max_index = 0;

            while (!reader.EndOfStream)
            {
                DPoint dataLine = new DPoint();
                List<Node> X = new List<Node>();
                string[] tmp = reader.ReadLine().Split(' ');

                for (int i = 0; i < tmp.Length; ++i)
                    if (tmp[i] == "")
                        tmp = tmp.Where(w => w != tmp[i]).ToArray();

                int m = tmp.Length - 1;
                Node[] x = new Node[m];

                dataLine.Label = double.Parse(tmp[0]);

                for (int i = 0; i < m; ++i)
                {
                    string[] t = tmp[i + 1].Split(':');
                    x[i] = new Node();
                    x[i].Index = int.Parse(t[0]);
                    x[i].Value = double.Parse(t[1]);
                }
                dataLine.MyNode = x.ToList();

                if (m > 0) max_index = Math.Max(max_index, x[m - 1].Index);

                data.Add(dataLine);
            }

            return new Dataset(data.ToArray());
        } 

        /// <summary>
        /// Read a Dataset from a File.
        /// </summary>
        /// <param name="filename">The path of the file to Read</param>
        /// <returns></returns>
        public static Dataset Read(string filename)
        {
            FileStream fStream = File.OpenRead(filename);

            try
            {
                return Read(fStream);
            }
            finally
            {
                fStream.Close();
            }
        }

        #endregion

    }



}
