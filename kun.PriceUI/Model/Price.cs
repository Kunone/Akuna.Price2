using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akuna.PriceService;
using kun.Infrastructure;
using System.Windows.Media;

namespace kun.PriceUI
{
    public sealed class Price : NotifyObject, IPrices, IEquatable<Price>
    {
        private double _askPx;
        private uint _askQty;
        private double _bidPx;
        private uint _bidQty;
        private uint _volume;
        private DateTime _lastUpdatedDate;
        private uint _instrumentID;
        private double _askPxOld;
        private double _bidPxOld;

        public double AskPx
        {
            get { return _askPx; }
            set
            {
                AskPxOld = Volume == 0 ? value : _askPx;
                if (value == _askPx) return;

                _askPx = value;
                NotifyPropertyChanged("AskPx");
            }
        }
        public double AskPxOld
        {
            get { return _askPxOld; }
            set
            {
                if (value == _askPxOld) return;

                _askPxOld = value;
                NotifyPropertyChanged("AskPxOld");
            }
        }

        public uint AskQty
        {
            get { return _askQty; }
            set
            {
                if (value == _askQty) return;
                _askQty = value;
                NotifyPropertyChanged("AskQty");
            }
        }

        public double BidPx
        {
            get { return _bidPx; }
            set
            {
                BidPxOld = Volume == 0 ? value : _bidPx;
                if (value == _bidPx) return;

                _bidPx = value;
                NotifyPropertyChanged("BidPx");
            }
        }
        public double BidPxOld
        {
            get { return _bidPxOld; }
            set
            {
                if (value == _bidPxOld) return;

                _bidPxOld = value;
                NotifyPropertyChanged("BidPxOld");
            }
        }

        public uint BidQty
        {
            get { return _bidQty; }
            set
            {
                if (value == _bidQty) return;
                _bidQty = value;
                NotifyPropertyChanged("BidQty");
            }
        }

        public uint Volume
        {
            get { return _volume; }
            set
            {
                if (value == _volume) return;
                _volume = value;
                NotifyPropertyChanged("Volume");
            }
        }

        public DateTime LastUpdatedDate
        {
            get { return _lastUpdatedDate; }
            set
            {
                if (value == _lastUpdatedDate) return;
                _lastUpdatedDate = value;
                //NotifyPropertyChanged("LastUpdatedDate");
            }
        }

        public uint InstrumentID
        {
            get { return _instrumentID; }
            set
            {
                if (value == _instrumentID) return;
                _instrumentID = value;
            }
        }

        private bool Equals(Price p)
        {
            return this.GetHashCode() == p.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var p = obj as Price;
            if (p == null) return false;
            return this.Equals(p);
        }
        public override int GetHashCode()
        {
            return _instrumentID.GetHashCode();
        }

        bool IEquatable<Price>.Equals(Price other)
        {
            return this.Equals(other);
        }
    }
}
