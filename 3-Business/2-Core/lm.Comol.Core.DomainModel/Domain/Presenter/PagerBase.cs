
using System;
namespace lm.Comol.Core.DomainModel
{
    [CLSCompliant(true), Serializable()]
    public class PagerBase
    {
        #region "Private Fields"
        private int _pageindex = 0;
        private int _previous = 0;
        private int _pagesize = 1;
        private int _lastpage = 0;
        private int _firstpage = 0;
        private bool _enabled = true;
        private bool _allowzerosize = false;
        private int _count;
        private int _delta = 4;
        private int _deltafirst;
        #endregion
        private int _deltalast;

        #region "Constructors"
        public PagerBase()
            : base()
        {
        }

        public PagerBase(int pagesize, int pageindex, int total = 0, int first = 0)
            : base()
        {
            this.PageIndex = pageindex;
            this.PageSize = pagesize;

            if (total == 0)
            {
                total = int.MaxValue;
            }
            this.LastPage = total;
        }
        #endregion

        #region "Public Properties"
            public int Skip
            {
                get { return this.PageSize * this.PageIndex; }
            }
            /// <summary>
            /// Set always FIRST than pageIndex
            /// </summary>
            public int PageSize
            {
                get
                {
                    if (Enabled)
                        return _pagesize;
                    else
                        return LastPage;
                }

                set
                {

                    if ((value > 0) | (AllowZeroSize))
                    {
                        int tmp = (int)Math.Floor((double)((_pageindex * _pagesize) / value));

                        LastPage = (int)Math.Floor((double)(Count / value));


                        if (LastPage > 1)
                        {
                            PageIndex = tmp;
                            _pagesize = value;

                            if (PageIndex + Delta > LastPage)
                                ScorriFinestra(false);
                            else
                            {
                                if (PageIndex - Delta < FirstPage)
                                    ScorriFinestra(true);
                                else
                                    ScorriFinestra();
                            }
                        }
                        else
                            _pagesize = value;
                    }
                }
            }

            /// <summary>
            /// Set always AFTER setting pageSize
            /// </summary>
            public int PageIndex
            {
                get
                {
                    if (Enabled)
                    {
                        return _pageindex;
                    }
                    else
                    {
                        return _firstpage;
                    }
                }
                set
                {
                    if ((value >= FirstPage))
                    {
                        if ((value <= LastPage))
                        {
                            _pageindex = value;
                        }
                        else
                        {
                            _pageindex = LastPage;
                        }
                    }
                    else
                    {
                        _pageindex = FirstPage;
                    }

                    if (DeltaFirst == 0 && DeltaLast == 0)
                    {
                        _deltafirst = Math.Max(0, _pageindex - _delta);
                        _deltalast = Math.Min(_lastpage, _pageindex + _delta);                        
                    }

                    CalcDelta();
                }
            }

        public int LastPage
        {
            get { return _lastpage; }
            set { _lastpage = value; }
        }

        public int FirstPage
        {
            get { return _firstpage; }
            set { _firstpage = value; }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public bool AllowZeroSize
        {
            get { return _allowzerosize; }
            set { _allowzerosize = value; }
        }

        public int OverrideZeroSize
        {
            set { PageSize = value; }
        }

        /// <summary>
        /// Count = if (count>0 ? count-1 : 0)
        /// </summary>
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                 LastPage = (int)Math.Floor((double)(_count/PageSize));
                //LastPage =  (_count/PageSize)   Math.Floor((double)(_count / PageSize));
                    //Math.Floor(((Double)value / (Double)PageSize));
            }
        }

        public int Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        public int DeltaFirst
        {
            get { return _deltafirst; }
        }
        public int DeltaLast
        {
            get { return _deltalast; }
        }
        #endregion

        #region "Methods"

        public void initialize()
        {
            _deltafirst = 0;
            _deltalast = 2 * Delta;
        }
        private int GlobalIndex()
        {
            return PageSize * PageIndex;
        }

        private void CalcDelta()
        {
            //If LastPage > 1 Then
            if (PageIndex <= FirstPage + Delta)
                ScorriFinestra(true);
            else if (PageIndex >= LastPage - Delta)
                ScorriFinestra(false);
            else
                ScorriFinestra();
        }

        //ByVal d As Integer)
        private void ScorriFinestra()
        {
            //if (DeltaLast != 0 || DeltaFirst != 0)
            //{
                int val = (DeltaLast + DeltaFirst) / 2 - PageIndex;
                //If d <= 0 Then
                //    _deltafirst += val
                //    _deltalast += val
                //Else
                //    _deltafirst -= val
                //    _deltalast -= val
                //End If
                _deltafirst -= val;
                _deltalast -= val;
            //}
        }

        private void ScorriFinestra(bool first)
        {
            if (first)
            {
                _deltafirst = FirstPage;
                _deltalast = FirstPage + 2 * Delta;
            }
            else
            {
                _deltalast = LastPage;
                _deltafirst = LastPage - 2 * Delta;
            }
        }

        public void GoFirst()
        {
            PageIndex = FirstPage;
            ScorriFinestra(true);
        }

        public void GoNext()
        {
            PageIndex += 1;
        }

        public void GoPrev()
        {
            PageIndex -= 1;
        }

        public void GoLast()
        {
            PageIndex = LastPage;
            ScorriFinestra(false);
        }

        public bool isLastPage()
        {
            return PageIndex == LastPage;
        }

        public bool isFirstPage()
        {
            return PageIndex == FirstPage;
        }

        public bool isSecondPage()
        {
            return PageIndex == FirstPage + 1;
        }

        public bool isPenultimatePage()
        {
            return PageIndex == LastPage - 1;
        }
        #endregion

    }
}