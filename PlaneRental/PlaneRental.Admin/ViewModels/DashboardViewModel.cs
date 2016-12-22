using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using PlaneRental.Client.Contracts;
using PlaneRental.Client.Entities;
using Core.Common.Contracts;
using Core.Common.UI.Core;

namespace PlaneRental.Admin.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DashboardViewModel : ViewModelBase
    {
        [ImportingConstructor]
        public DashboardViewModel(IServiceFactory serviceFactory)
        {
            _ServiceFactory = serviceFactory;
        }

        IServiceFactory _ServiceFactory;
        
        public override string ViewTitle
        {
            get { return "Dashboard"; }
        }

        protected override void OnViewLoaded()
        {
            // can check properties for null here if not want to re-get every time view shows
            
            WithClient<IInventoryService>(_ServiceFactory.CreateClient<IInventoryService>(), async inventoryClient =>
            {
                Planes = await inventoryClient.GetAllPlanesAsync();
            });
        }

        Plane[] _Planes;
        CustomerRentalData[] _CurrentlyRented;

        public Plane[] Planes
        {
            get { return _Planes; }
            set
            {
                if (_Planes != value)
                {
                    _Planes = value;
                    OnPropertyChanged(() => Planes, false);
                }
            }
        }
    }
}
