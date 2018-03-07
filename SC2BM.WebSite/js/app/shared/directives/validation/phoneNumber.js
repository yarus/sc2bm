(function () {
	'use strict';

	angular.module('Sc2bmApp').directive('phoneNumber', phoneNumber);

	phoneNumber.$inject = ['helperService'];

	function phoneNumber(helperService) {

		//region Directive declaration
		var directive = {
			require: '?ngModel',
			link: function (scope, element, attrs, ngModelCtrl) {
				if (!ngModelCtrl) {
					return;
				}
				
				ngModelCtrl.$parsers.push(function (val) {
					if (angular.isUndefined(val)) {
						val = '';
					}

					var clean = helperService.formatPhone('US', val);

					if (val !== clean) {
						ngModelCtrl.$setViewValue(clean);
						ngModelCtrl.$render();
					}

					return clean;
				});
			}
		};

		return directive;
		//endregion
	}
})();