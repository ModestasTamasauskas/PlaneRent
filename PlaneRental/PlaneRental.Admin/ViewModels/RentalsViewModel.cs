using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using PlaneRental.Client.Contracts;
using PlaneRental.Client.Entities;
using Core.Common;
using Core.Common.Contracts;
using Core.Common.Extensions;
using Core.Common.UI.Core;

namespace PlaneRental.Admin.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RentalsViewModel : ViewModelBase
    {
        [ImportingConstructor]
        public RentalsViewModel(IServiceFactory serviceFactory)
        {
            _ServiceFactory = serviceFactory;

            AcceptRentalReturnCommand = new DelegateCommand<int>(OnAcceptRentalReturnExecute);
        }

        IServiceFactory _ServiceFactory;

        public override string ViewTitle
        {
            get { return "Current Rentals"; }
        }

        ObservableCollection<CustomerRentalData> _Rentals;

        public DelegateCommand<int> AcceptRentalReturnCommand { get; private set; }

        public event EventHandler<ErrorMessageEventArgs> ErrorOccured;
        public event EventHandler RentalReturned;
        
        public ObservableCollection<CustomerRentalData> Rentals
        {
            get { return _Rentals; }
            set
            {
                if (_Rentals != value)
                {
                    _Rentals = value;
                    OnPropertyChanged(() => Rentals, false);
                }
            }
        }

        protected override void OnViewLoaded()
        {
            _Rentals = new ObservableCollection<CustomerRentalData>();

            WithClient<IRentalService>(_ServiceFactory.CreateClient<IRentalService>(), async rentalClient =>
            {
                CustomerRentalData[] rentals = await rentalClient.GetCurrentRentalsAsync();
                if (rentals != null)
                {
                    // convert returned data into observable collection so binding can refresh automatically
                    Rentals.Merge(rentals);
                }
            });
        }

        void OnAcceptRentalReturnExecute(int rentalId)
        {
            WithClient<IRentalService>(_ServiceFactory.CreateClient<IRentalService>(), rentalClient =>
            {
                CustomerRentalData customerRentalData = _Rentals.Where(item => item.RentalId == rentalId).FirstOrDefault();
                if (customerRentalData != null)
                {
                    try
                    {
                        Rental rental = rentalClient.GetRentalAsync(rentalId).Result;
                        if (rental != null)
                        {
                            rentalClient.AcceptPlaneReturnAsync(rental.PlaneId);
                            Rentals.Remove(customerRentalData);

                            if (RentalReturned != null)
                                RentalReturned(this, EventArgs.Empty);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ErrorOccured != null)
                            ErrorOccured(this, new ErrorMessageEventArgs(ex.Message));
                    }
                }
            });
        }
    }
}
