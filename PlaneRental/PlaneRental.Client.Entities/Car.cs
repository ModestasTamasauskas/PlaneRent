using System;
using System.Collections.Generic;
using System.Linq;
using Core.Common.Core;
using FluentValidation;

namespace PlaneRental.Client.Entities
{
    public class Plane : ObjectBase
    {
        int _PlaneId;
        string _Description;
        string _Color;
        int _Year;
        decimal _RentalPrice;
        bool _CurrentlyRented;

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

        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    OnPropertyChanged(() => Description);
                }
            }
        }

        public string Color
        {
            get { return _Color; }
            set
            {
                if (_Color != value)
                {
                    _Color = value;
                    OnPropertyChanged(() => Color);
                }
            }
        }

        public int Year
        {
            get { return _Year; }
            set
            {
                if (_Year != value)
                {
                    _Year = value;
                    OnPropertyChanged(() => Year);
                }
            }
        }

        public decimal RentalPrice
        {
            get { return _RentalPrice; }
            set
            {
                if (_RentalPrice != value)
                {
                    _RentalPrice = value;
                    OnPropertyChanged(() => RentalPrice);
                }
            }
        }

        public bool CurrentlyRented
        {
            get { return _CurrentlyRented; }
            set
            {
                if (_CurrentlyRented != value)
                {
                    _CurrentlyRented = value;
                    OnPropertyChanged(() => CurrentlyRented);
                }
            }
        }

        public string LongDescription
        {
            get
            {
                return string.Format("{0} {1} {2}", _Year, _Color, _Description);
            }
        }

        class PlaneValidator : AbstractValidator<Plane>
        {
            public PlaneValidator()
            {
                RuleFor(obj => obj.Description).NotEmpty();
                RuleFor(obj => obj.Color).NotEmpty();
                RuleFor(obj => obj.RentalPrice).GreaterThan(0);
                RuleFor(obj => obj.Year).GreaterThan(2000).LessThanOrEqualTo(DateTime.Now.Year + 1);
            }
        }

        protected override IValidator GetValidator()
        {
            return new PlaneValidator();
        }
    }
}
