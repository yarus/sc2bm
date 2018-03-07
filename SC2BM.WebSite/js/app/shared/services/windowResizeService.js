(function() {
	'use strict';

	angular.module('Sc2bmApp').service('windowResizeService', windowResizeService);

	windowResizeService.$inject = ['$rootScope'];

	function windowResizeService($rootScope) {
		var isBound = false;
		var resizeTimeout;

		function addListener() {
			if (!isBound) {
				angular.element(window).bind('resize', function () {
					clearTimeout(resizeTimeout);
					resizeTimeout = setTimeout(function () {
						$rootScope.$broadcast('window-resize');
					}, 100);
				});
				isBound = true;
			}
		}

		return {
			addListener: addListener
		};
	}
})();