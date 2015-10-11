using System;
using System.Collections.Generic;
using DataMining;

namespace DataMining
{
    public class Node
    {
        #region Variable
        private int _index;
        private double _value;
        #endregion

        #region Properties

        /// <summary>
        /// Index of Node
        /// </summary>
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                if (value != _index)
                    _index = value;
            }
        }

        /// <summary>
        /// Value of Index
        /// </summary>
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value != _value)
                    _value = value;
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Node()
        {
        }

        /// <summary>
        /// Constructor 1
        /// </summary>
        /// <param name="Index">Index of Value</param>
        /// <param name="Value">Value to Store</param>
        public Node(int Index, double Value)
        {
            _index = Index;
            _value = Value;
        }


        #endregion
    }


    public class DPoint
    {
        #region Variable

        private double _label;
        private List<Node> _node;

        #endregion

        #region Properties

        public double Label
        {
            get
            {
                return _label;
            }
            set
            {
                if (value != _label)
                    _label = value;
            }
        }

        public List<Node> MyNode
        {
            get
            {
                return _node;
            }
            set
            {
                if (value != _node)
                    _node = value;
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public  DPoint()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_label">Label of DPoint</param>
        /// <param name="_node"></param>
        public DPoint(double _label, List<Node> _node)
        {
            this._label = _label;
            this._node = _node;
        }

        /// <summary>
        /// Overload operator + for DPoint
        /// </summary>
        /// <param name="d1">Vector d1</param>
        /// <param name="d2">vector d2</param>
        /// <returns>sum of d1 & d2</returns>
        public static DPoint operator +(DPoint d1, DPoint d2)
        {
            int d =0 ;
            d = d1.MyNode.Count;
            DPoint dp = new DPoint();
            dp.MyNode = new List<Node>();

            for (int i =0; i<d; ++i)
            {
                dp.MyNode.Add(new Node(d1.MyNode[i].Index, d1.MyNode[i].Value + d2.MyNode[i].Value));
            }

            return dp;
        }

        /// <summary>
        /// Overload operator - for DPoint
        /// </summary>
        /// <param name="d1">Vector d1</param>
        /// <param name="d2">vector d2</param>
        /// <returns>minus of d1 & d2</returns>
        public static DPoint operator -(DPoint d1, DPoint d2)
        {
            int d = d1.MyNode.Count;
            DPoint dp = new DPoint();
            dp.MyNode = new List<Node>();

            for (int i = 0; i < d; ++i)
            {
                dp.MyNode.Add(new Node(d1.MyNode[i].Index, d1.MyNode[i].Value - d2.MyNode[i].Value));
            }

            return dp;
        }

        public static DPoint operator /(DPoint d1, double num)
        {
            int d = d1.MyNode.Count;
            DPoint dp = new DPoint();
            dp.MyNode = new List<Node>();

            for (int i = 0; i < d; ++i)
            {
                dp.MyNode.Add(new Node(d1.MyNode[i].Index, (double)d1.MyNode[i].Value/num));
            }

            return dp;
        }


        #endregion
    }

}
