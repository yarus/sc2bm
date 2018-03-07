(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('confirmRegistrationController', confirmRegistrationController);

	confirmRegistrationController.$inject = ['$routeParams', '$location', 'userService'];

	function confirmRegistrationController($routeParams, $location, userService) {
		//region ViewModel declaration
		/*jshint validthis:true */
	    var vm = this;
	    vm.confirmationSalt = "";
	    vm.confirmationSaltParam = "";
	    vm.confirmationStatus = "Confirmation process...";
	    vm.userName = "";

	    vm.confirmRegistration = confirmRegistration;

		activate();
		//endregion

		//region Methods
		function activate() {
		    vm.userName = $routeParams.userName;
		    vm.confirmationSaltParam = $routeParams.confirmationSalt;

            if (vm.userName == null || vm.userName == "") {
                alert("Username was not provided!");
                $location.path("/home");
                return;
            }

            if (vm.userName != null && vm.confirmationSaltParam != null) {
                vm.confirmationSalt = vm.confirmationSaltParam;
                confirmRegistration();
            }
		}

		function confirmRegistration() {
		    if (vm.userName == null || vm.userName == "") {
		        alert("Username was not provided!");
		        return;
		    }

		    if (vm.confirmationSalt == null || vm.confirmationSalt == "") {
		        alert("Confirmation code was not provided!");
		        return;
		    }

		    userService.confirmRegistration(vm.userName, vm.confirmationSalt)
                .success(function (result) {
                    alert("Registration for user " + vm.userName + " was confirmed!");
                    $location.path("/home");
                })
                .error(function (result) {
                    alert("Confirmation for user " + vm.userName + " was not completed. Please try again later.");
                });
		}
	}
})();