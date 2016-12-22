
appMainModule.controller("CurrentReservationsViewModel", function ($scope, $http, viewModelHelper) {

    $scope.viewModelHelper = viewModelHelper;
    $scope.reservations = [];

    $scope.loadOpenReservations = function () {
        viewModelHelper.apiGet('api/reservation/getopen', null,
            function (result) {
                for (var i = 0; i < result.length; i++) {
                    result[i]['CancelRequest'] = false;
                }
                $scope.reservations = result;
            });
    }

    $scope.requestCancelReservation = function (reservation) {
        reservation.CancelRequest = true;
    }

    $scope.undoCancelRequest = function (reservation) {
        reservation.CancelRequest = false;
    }

    $scope.cancelReservation = function (reservation) {
        viewModelHelper.apiGet('api/reservation/cancel', { 'reservationId': reservation.ReservationId },
            function (result) {
                var index = $scope.reservations.indexOf(reservation);
                $scope.reservations.splice(index, 1);
            });
    }

    $scope.loadOpenReservations();
});
