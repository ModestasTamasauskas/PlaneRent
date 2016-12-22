
(function (cr) {
    var MyAccountModel = function (accountId, loginEmail, firstName, lastName, address, city, state, zipCode, creditPlaned, expDate) {

        var self = this;

        self.AccountId = accountId; // not going to display, just a place to store this
        self.LoginEmail = loginEmail; // // not going to display, just a place to store this
        self.FirstName = firstName;
        self.LastName = lastName;
        self.Address = address;
        self.City = city;
        self.State = state;
        self.ZipCode = zipCode;
        self.CreditPlaned = creditPlaned;
        self.ExpDate = expDate.substring(0, 2) + "/" + expDate.substring(2, 4);
        
    }
    cr.MyAccountModel = MyAccountModel;
}(window.PlaneRental));
