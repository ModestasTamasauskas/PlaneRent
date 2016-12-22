
(function (cr) {
    var ReservationModel = function (reservationId, car, rentalDate, returnDate) {

        var self = this;

        self.ReservationId = ko.observable(reservationId);
        self.Plane = ko.observable(car);
        self.RentalDate = ko.observable(rentalDate);
        self.ReturnDate = ko.observable(returnDate);

        self.CancelRequest = ko.observable(false);
    }
    cr.ReservationModel = ReservationModel;
}(window.PlaneRental));
