(function () {
	'use strict';

	angular.module('Sc2bmApp').directive('backButton', backButton);

	function backButton() {
		//region Directive declaration
		var directive = {
			restrict: 'A',
			link: function (scope, element, attrs) {
				element.bind('click', goBack);

				function goBack() {
					history.back();
					scope.$apply();
				}
			}
		};

		return directive;
		//endregion
	}
})();