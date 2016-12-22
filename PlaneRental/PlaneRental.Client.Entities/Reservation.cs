using System;
using System.Collections.Generic;
using System.Linq;
using Core.Common.Core;

namespace PlaneRental.Client.Entities
{
    public class Reservation : ObjectBase
    {
        int _ReservationId;
        string _AccountId;
        int _PlaneId;
        DateTime _ReturnDate;
        DateTime _RentalDate;

        public int ReservationId
        {
            get { return _ReservationId; }
            set
            {
                if (_ReservationId != value)
                {
                    _ReservationId = value;
                    OnPropertyChanged(() => ReservationId);
                }
            }
        }

        public string AccountId
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

        public DateTime RentalDate
        {
            get { return _RentalDate; }
            set
            {
                if (_RentalDate != value)
                {
                    _RentalDate = value;
                    OnPropertyChanged(() => RentalDate);
                }
            }
        }

        public DateTime ReturnDate
        {
            get { return _ReturnDate; }
            set
            {
                if (_ReturnDate != value)
                {
                    _ReturnDate = value;
                    OnPropertyChanged(() => ReturnDate);
                }
            }
        }
    }
}
