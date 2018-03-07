(function () {
    'use strict';

    angular.module('Sc2bmApp').controller('registerController', registerController);

    registerController.$inject = ['$location', 'session', 'userService', 'helperService'];

    function registerController($location, session, userService, helperService) {
        //region ViewModel declaration
        /*jshint validthis:true */
        var vm = this;

        vm.user = {};

        vm.dateOptions = null;
        vm.register = register;
        vm.userNameChanged = userNameChanged;
        vm.passwordChanged = passwordChanged;
        vm.emailChanged = emailChanged;
        vm.repeatPasswordChanged = repeatPasswordChanged;
        
        activate();
        //endregion

        //region Methods
        function activate() {
            vm.dateOptions = helperService.getDefaultDatePickerOptions();
        }

        function register(user) {
            if (vm.registerForm.$invalid) {
                return;
            }

            userService.registerUser(user)
                .success(function (result) {
                    user.ID = result;
                    $location.path('/confirmRegistration/' + user.UserName);
                })
                .error(function (result) {
                    alert("Error: " + result);
                });
        }

        function userNameChanged(value) {
            vm.registerForm.userName.$setValidity('unique', true);

            if (vm.registerForm.userName.$invalid) {
                return;
            }

            userService.getCountByUserName(value)
                .success(function (result) {
                    if (result > 0) {
                        vm.registerForm.userName.$setValidity('unique', false);
                    }
                });
        }

        function emailChanged(value) {
            vm.registerForm.emailAddress.$setValidity('unique', true);

            if (vm.registerForm.emailAddress.$invalid) {
                return;
            }

            userService.getCountByEmail(value)
                .success(function (result) {
                    if (result > 0) {
                        vm.registerForm.emailAddress.$setValidity('unique', false);
                    }
                });
        }

        function repeatPasswordChanged(value) {
            vm.registerForm.repeatPassword.$setValidity('sameAsPassword', true);

            if (vm.user.Password != value) {
                vm.registerForm.repeatPassword.$setValidity('sameAsPassword', false);
            }
        }

        function passwordChanged() {
            vm.user.RepeatPassword = "";
        }
    }
})();