using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using PlaneRental.Client.Contracts;
using PlaneRental.Client.Entities;
using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.UI.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Core.Common;

namespace PlaneRental.Admin.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MaintainPlanesViewModel : ViewModelBase
    {
        [ImportingConstructor]
        public MaintainPlanesViewModel(IServiceFactory serviceFactory)
        {
            _ServiceFactory = serviceFactory;

            EditPlaneCommand = new DelegateCommand<Plane>(OnEditPlaneCommand);
            DeletePlaneCommand = new DelegateCommand<Plane>(OnDeletePlaneCommand);
            AddPlaneCommand = new DelegateCommand<object>(OnAddPlaneCommand);
        }

        IServiceFactory _ServiceFactory;

        EditPlaneViewModel _CurrentPlaneViewModel;

        public DelegateCommand<Plane> EditPlaneCommand { get; private set; }
        public DelegateCommand<Plane> DeletePlaneCommand { get; private set; }
        public DelegateCommand<object> AddPlaneCommand { get; private set; }
        
        public override string ViewTitle
        {
            get { return "Maintain Planes"; }
        }

        public event CancelEventHandler ConfirmDelete;
        public event EventHandler<ErrorMessageEventArgs> ErrorOccured;

        public EditPlaneViewModel CurrentPlaneViewModel
        {
            get { return _CurrentPlaneViewModel; }
            set
            {
                if (_CurrentPlaneViewModel != value)
                {
                    _CurrentPlaneViewModel = value;
                    OnPropertyChanged(() => CurrentPlaneViewModel, false);
                }
            }
        }

        ObservableCollection<Plane> _Planes;

        public ObservableCollection<Plane> Planes
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
        
        protected override void OnViewLoaded()
        {
            _Planes = new ObservableCollection<Plane>();

            WithClient<IInventoryService>(_ServiceFactory.CreateClient<IInventoryService>(), async inventoryClient =>
            {
                Plane[] Planes = await inventoryClient.GetAllPlanesAsync();
                if (Planes != null)
                {
                    foreach (Plane Plane in Planes)
                        _Planes.Add(Plane);
                }
            });
        }

        void OnEditPlaneCommand(Plane Plane)
        {
            if (Plane != null)
            {
                CurrentPlaneViewModel = new EditPlaneViewModel(_ServiceFactory, Plane);
                CurrentPlaneViewModel.PlaneUpdated += CurrentPlaneViewModel_PlaneUpdated;
                CurrentPlaneViewModel.CancelEditPlane += CurrentPlaneViewModel_CancelEvent;
            }
        }

        void OnAddPlaneCommand(object arg)
        {
            Plane Plane = new Plane();
            CurrentPlaneViewModel = new EditPlaneViewModel(_ServiceFactory, Plane);
            CurrentPlaneViewModel.PlaneUpdated += CurrentPlaneViewModel_PlaneUpdated;
            CurrentPlaneViewModel.CancelEditPlane += CurrentPlaneViewModel_CancelEvent;
        }

        void CurrentPlaneViewModel_PlaneUpdated(object sender, Support.PlaneEventArgs e)
        {
            if (!e.IsNew)
            {
                Plane Plane = _Planes.Where(item => item.PlaneId == e.Plane.PlaneId).FirstOrDefault();
                if (Plane != null)
                {
                    Plane.Description = e.Plane.Description;
                    Plane.Color = e.Plane.Color;
                    Plane.Year = e.Plane.Year;
                    Plane.RentalPrice = e.Plane.RentalPrice;
                }
            }
            else
                _Planes.Add(e.Plane);

            CurrentPlaneViewModel = null;
        }

        void CurrentPlaneViewModel_CancelEvent(object sender, EventArgs e)
        {
            CurrentPlaneViewModel = null;
        }

        void OnDeletePlaneCommand(Plane Plane)
        {
            bool PlaneIsRented = false;

            // check to see if Plane is currently rented
            WithClient<IRentalService>(_ServiceFactory.CreateClient<IRentalService>(), async rentalClient =>
            {
                PlaneIsRented = await rentalClient.IsPlaneCurrentlyRentedAsync(Plane.PlaneId);
            });

            if (!PlaneIsRented)
            {
                CancelEventArgs args = new CancelEventArgs();
                if (ConfirmDelete != null)
                    ConfirmDelete(this, args);

                if (!args.Cancel)
                {
                    WithClient<IInventoryService>(_ServiceFactory.CreateClient<IInventoryService>(), async inventoryClient =>
                    {
                        await inventoryClient.DeletePlaneAsync(Plane.PlaneId);
                        _Planes.Remove(Plane);
                    });
                }
            }
            else
            {
                if (ErrorOccured != null)
                    ErrorOccured(this, new ErrorMessageEventArgs("Cannot delete this Plane. It is currently rented."));
            }
        }
    }
}
