
reservePlaneModule.controller("ReservePlaneViewModel", function ($scope, viewModelHelper) {

    $scope.viewModelHelper = viewModelHelper;
    $scope.reservePlaneModel = new PlaneRental.ReservePlaneModel();
    $scope.castro = { propA: 'A', propB: 'B' };
});


reservePlaneModule.controller("ReserveViewModel", function ($scope, $http, $location, viewModelHelper) {

    var reservePlaneModelRules = [];

    var setupRules = function () {
        reservePlaneModelRules.push(new viewModelHelper.PropertyRule("PickupDate", {
            required: { message: "Pickup date is required" }
        }));
        reservePlaneModelRules.push(new viewModelHelper.PropertyRule("ReturnDate", {
            required: { message: "Return date is required" }
        }));
    }

    $scope.reservePlaneModel.initialized = true;
    
    $scope.submit = function () {
        if ($scope.reservePlaneModel.PickupDate != null && $scope.reservePlaneModel.PickupDate != '')
            $scope.reservePlaneModel.PickupDate = moment($scope.reservePlaneModel.PickupDate).format('MM/DD/YYYY');
        else
            $scope.reservePlaneModel.PickupDate = '';
        if ($scope.reservePlaneModel.ReturnDate != null && $scope.reservePlaneModel.ReturnDate != '')
            $scope.reservePlaneModel.ReturnDate = moment($scope.reservePlaneModel.ReturnDate).format('MM/DD/YYYY');
        else
            $scope.reservePlaneModel.ReturnDate = '';
        
        viewModelHelper.validateModel($scope.reservePlaneModel, reservePlaneModelRules);
        viewModelHelper.modelIsValid = $scope.reservePlaneModel.isValid;
        viewModelHelper.modelErrors = $scope.reservePlaneModel.errors;
        if (viewModelHelper.modelIsValid) {
            $location.path(PlaneRental.rootPath + 'customer/reserve/planelist');
        }
        else
            viewModelHelper.modelErrors = $scope.reservePlaneModel.errors;
    }

    $scope.openPickup = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.openedPickup = true;
    }

    $scope.openReturn = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.openedReturn = true;
    }

    setupRules();
});


reservePlaneModule.controller("PlaneListViewModel", function ($scope, $http, $location, viewModelHelper) {

    $scope.viewMode = 'planelist'; 
    $scope.planes = [];
    $scope.reservationNumber = '';
    
    $scope.availablePlanes = function () {
        if (!$scope.reservePlaneModel.initialized) {
            $location.path(PlaneRental.rootPath + 'customer/reserve');
        }
        viewModelHelper.apiGet('api/reservation/availableplanes',
                { pickupDate: $scope.reservePlaneModel.PickupDate, returnDate: $scope.reservePlaneModel.ReturnDate },
            function (result) {
                $scope.planes = result;
            });    
    }

    $scope.selectPlane = function (plane) {
        var model = { PickupDate: $scope.reservePlaneModel.PickupDate, ReturnDate: $scope.reservePlaneModel.ReturnDate, Plane: plane.PlaneId };
        viewModelHelper.apiPost('api/reservation/reserveplane', model,
            function (result) {
                $scope.reservationNumber = result.data.ReservationId;
                $scope.viewMode = 'success';
            });
    }

    $scope.availablePlanes();
});
