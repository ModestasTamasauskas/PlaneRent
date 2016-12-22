using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using PlaneRental.Client.Contracts;
using PlaneRental.Client.Entities;
using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.UI.Core;
using PlaneRental.Admin.Support;

namespace PlaneRental.Admin.ViewModels
{
    public class EditPlaneViewModel : ViewModelBase
    {
        // note that this viewmodel is instantiated on-demand from parent and not with DI

        public EditPlaneViewModel(IServiceFactory serviceFactory, Plane Plane)
        {
            _ServiceFactory = serviceFactory;
            _Plane = new Plane()
            {
                PlaneId = Plane.PlaneId,
                Description = Plane.Description,
                Color = Plane.Color,
                Year = Plane.Year,
                RentalPrice = Plane.RentalPrice
            };

            _Plane.CleanAll();

            SaveCommand = new DelegateCommand<object>(OnSaveCommandExecute, OnSaveCommandCanExecute);
            CancelCommand = new DelegateCommand<object>(OnCancelCommandExecute);
        }

        IServiceFactory _ServiceFactory;
        Plane _Plane;

        public DelegateCommand<object> SaveCommand { get; private set; }
        public DelegateCommand<object> CancelCommand { get; private set; }

        public event EventHandler CancelEditPlane;
        public event EventHandler<PlaneEventArgs> PlaneUpdated;

        public Plane Plane
        {
            get { return _Plane; }
        }

        protected override void AddModels(List<ObjectBase> models)
        {
            models.Add(Plane);
        }
        
        void OnSaveCommandExecute(object arg)
        {
            ValidateModel();

            if (IsValid)
            {
                WithClient<IInventoryService>(_ServiceFactory.CreateClient<IInventoryService>(), async inventoryClient =>
                {
                    bool isNew = (_Plane.PlaneId == 0);

                    var savedPlane = await inventoryClient.UpdatePlaneAsync(_Plane);
                    if (savedPlane != null)
                    {
                        if (PlaneUpdated != null)
                            PlaneUpdated(this, new PlaneEventArgs(savedPlane, isNew));
                    }
                });
            }
        }
        
        bool OnSaveCommandCanExecute(object arg)
        {
            return _Plane.IsDirty;
        }

        void OnCancelCommandExecute(object arg)
        {
            if (CancelEditPlane != null)
                CancelEditPlane(this, EventArgs.Empty);
        }
    }
}
