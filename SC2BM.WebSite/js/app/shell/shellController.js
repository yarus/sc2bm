(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('shellController', shellController);

	shellController.$inject = ['$rootScope', '$location', '$locale', '$routeParams', '$window', 'session', 'commentRepository', 'rateRepository'];

	function shellController($rootScope, $location, $locale, $routeParams, $window, session, commentRepository, rateRepository) {
		//region ViewModel declaration
	    var vm = this; // jshint ignore:line

		$locale.DATETIME_FORMATS.medium = 'MM/dd/yyyy, h:mma';
		$locale.DATETIME_FORMATS.mediumDate = 'MM/dd/yyyy';

		vm.deviceModeClass = '';

		vm.navigateToTour = navigateToTour;
		vm.init = init;

		activate();
		//endregion

		//region Methods
		function activate() {
		    //loadUser();

		    //var path = $location.path();
		    //$location.path(path);
		}

		function init(serverState) {
		    if (serverState != null) {
		        if (serverState.User != null) {
		            session.setUser(serverState.User);
		        }

                if (serverState.Ip != null) {
                    session.setIp(serverState.Ip);
                }
		    }

		    commentRepository.reset();
		    rateRepository.reset();
		}

        function navigateToTour() {
            $window.location.href = "/apptour";
        }
	}
})();