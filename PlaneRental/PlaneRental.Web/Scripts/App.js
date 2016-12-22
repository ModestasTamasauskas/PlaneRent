
var commonModule = angular.module('common', ['ngRoute','ui.bootstrap']);

var appMainModule = angular.module('appMain', ['common']);

var reservePlaneModule = angular.module('reservePlane', ['common'])
    .config(function ($routeProvider, $locationProvider) {
        $routeProvider.when(PlaneRental.rootPath + 'customer/reserve', { templateUrl: PlaneRental.rootPath + 'Templates/Reserve.html', controller: 'ReserveViewModel' });
        $routeProvider.when(PlaneRental.rootPath + 'customer/reserve/planelist', { templateUrl: PlaneRental.rootPath + 'Templates/PlaneList.html', controller: 'PlaneListViewModel' });
        $routeProvider.otherwise({ redirectTo: PlaneRental.rootPath + 'customer/reserve' });
        $locationProvider.html5Mode(true);
    });

var accountRegisterModule = angular.module('accountRegister', ['common'])
    .config(function ($routeProvider, $locationProvider) {
        $routeProvider.when(PlaneRental.rootPath + 'account/register/step1', { templateUrl: PlaneRental.rootPath + 'Templates/RegisterStep1.html', controller: 'AccountRegisterStep1ViewModel' });
        $routeProvider.when(PlaneRental.rootPath + 'account/register/step2', { templateUrl: PlaneRental.rootPath + 'Templates/RegisterStep2.html', controller: 'AccountRegisterStep2ViewModel' });
        $routeProvider.when(PlaneRental.rootPath + 'account/register/step3', { templateUrl: PlaneRental.rootPath + 'Templates/RegisterStep3.html', controller: 'AccountRegisterStep3ViewModel' });
        $routeProvider.when(PlaneRental.rootPath + 'account/register/confirm', { templateUrl: PlaneRental.rootPath + 'Templates/RegisterConfirm.html', controller: 'AccountRegisterConfirmViewModel' });
        $routeProvider.otherwise({ redirectTo: PlaneRental.rootPath + 'account/register/step1' });
        $locationProvider.html5Mode(true);
    });

commonModule.directive('jqdatepicker', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.datepicker({
                changeYear: "true",
                changeMonth: true,
                dateFormat: 'mm-dd-yyyy',
                showOtherMonths: true,
                showButtonPanel: true,
                onClose: function (selectedDate) {
                    scope.dateTtime = selectedDate + "T" + scope.time;
                    scope.$apply();
                }
            });
        }
    };
})

commonModule.factory('viewModelHelper', function ($http, $q) {
    return PlaneRental.viewModelHelper($http, $q);
});

(function (cr) {
    var initialId;
    cr.initialId = initialId;
}(window.PlaneRental));

(function (cr) {
    var initialState;
    cr.initialState = initialState;
}(window.PlaneRental));


(function (cr) {
    var mustEqual = function (val, other) {
        return val == other();
    }
    cr.mustEqual = mustEqual;
}(window.PlaneRental));

(function (cr) {
    var viewModelHelper = function ($http, $q) {
        
        var self = this;

        self.modelIsValid = ko.observable(true);
        self.modelErrors = ko.observableArray();
        self.isLoading = ko.observable(false);

        self.statePopped = false;
        self.stateInfo = {};

        self.apiGet = function (uri, data, success, failure, always) {
            self.isLoading = true;
            self.modelIsValid = true;
            $.ajax({ url: PlaneRental.rootPath + uri, data: data, async: false })
                .then(function (result) {
                    success(result);
                    if (always != null)
                        always();
                    self.isLoading = false;
                }, function (result) {
                    if (failure == null) {
                        if (result.status != 400)
                            self.modelErrors = [result.status + ':' + result.statusText + ' - ' + result.data.Message];
                        else
                            self.modelErrors = [result.data.Message];
                        self.modelIsValid = false;
                    }
                    else
                        failure(result);
                    if (always != null)
                        always();
                    self.isLoading = false;
                });
        }

        self.apiPost = function (uri, data, success, failure, always) {
            self.isLoading = true;
            self.modelIsValid = true;
            $http.post(PlaneRental.rootPath + uri, data)
                .then(function (result) {
                    success(result);
                    if (always != null)
                        always();
                    self.isLoading = false;
                }, function (result) {
                    if (failure == null) {
                        if (result.status != 400)
                            self.modelErrors = [result.status + ':' + result.statusText + ' - ' + result.data.Message];
                        else
                            self.modelErrors = [result.data.Message];
                        self.modelIsValid = false;
                    }
                    else
                        failure(result);
                    if (always != null)
                        always();
                    isLoading = false;
                });
        }

        self.PropertyRule = function (propertyName, rules) {
            var self = this;
            self.PropertyName = propertyName;
            self.Rules = rules;
        };

        self.validateModel = function (model, propertyRules) {
            var errors = [];
            var props = Object.keys(model);
            for (var i = 0; i < props.length; i++) {
                var prop = props[i];
                for (var j = 0; j < propertyRules.length; j++) {
                    var propertyRule = propertyRules[j];
                    if (prop == propertyRule.PropertyName) {
                        var rules = propertyRule.Rules;
                        if (rules.hasOwnProperty('required')) {
                            if (model[prop].trim() == '') {
                                errors.push(getMessage(rules.required));
                            }
                        }
                        if (rules.hasOwnProperty('pattern')) {
                            var regExp = new RegExp(rules.pattern.value);
                            if (regExp.exec(model[prop].trim()) == null) {
                                errors.push(getMessage(rules.pattern));
                            }
                        }
                        if (rules.hasOwnProperty('minLength')) {
                            var minLength = rules.minLength.value;
                            if (model[prop].trim().length < minLength) {
                                errors.push(getMessage(rules.minLength));
                            }
                        }
                    }
                }
            }

            model['errors'] = errors;
            model['isValid'] = (errors.length == 0);
        }

        var getMessage = function (rule) {
            var message = '';
            if (rule.hasOwnProperty('message'))
                message = rule.message;
            else
                message = prop + ' is invalid.';
            return message;
        }

        return this;
    }
    cr.viewModelHelper = viewModelHelper;
}(window.PlaneRental));
