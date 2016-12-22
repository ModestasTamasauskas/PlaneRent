using System;
using System.Collections.Generic;
using System.Linq;
using Core.Common.Core;

namespace PlaneRental.Client.Entities
{
    public class Rental : ObjectBase
    {
        int _RentalId;
        int _AccountId;
        int _PlaneId;
        DateTime _DateRented;
        DateTime _DateDue;
        DateTime? _DateReturned;

        public int RentalId
        {
            get { return _RentalId; }
            set
            {
                if (_RentalId != value)
                {
                    _RentalId = value;
                    OnPropertyChanged(() => RentalId);
                }
            }
        }

        public int AccountId
        {
            get { return _AccountId; }
            set
            {
                if (_AccountId != value)
                {
                    _AccountId = value;
                    OnPropertyChanged(() => AccountId);
                }
            }
        }

        public int PlaneId
        {
            get { return _PlaneId; }
            set
            {
                if (_PlaneId != value)
                {
                    _PlaneId = value;
                    OnPropertyChanged(() => PlaneId);
                }
            }
        }

        public DateTime DateRented
        {
            get { return _DateRented; }
            set
            {
                if (_DateRented != value)
                {
                    _DateRented = value;
                    OnPropertyChanged(() => DateRented);
                }
            }
        }

        public DateTime DateDue
        {
            get { return _DateDue; }
            set
            {
                if (_DateDue != value)
                {
                    _DateDue = value;
                    OnPropertyChanged(() => DateDue);
                }
            }
        }

        public DateTime? DateReturned
        {
            get { return _DateReturned; }
            set
            {
                if (_DateReturned != value)
                {
                    _DateReturned = value;
                    OnPropertyChanged(() => DateReturned);
                }
            }
        }
    }
}
