(function () {
	'use strict';

	angular.module('Sc2bmApp').directive('phoneLink', phoneLink);

	phoneLink.$inject = ['helperService'];

	function phoneLink(helperService) {
		//region Directive declaration
		var directive = {
			restrict: 'E',
			replace: true,
			transclude: true,
			template: '<a href="tel:{{phone}}">{{phone}}</a>',
			scope: {
				phone: '='
			},
			link: function(scope, element, attrs) {
				scope.$watch('phone', function (rawPhone) {
					if (rawPhone) {
						scope.phone = helperService.formatPhone('US', rawPhone);
					} else {
						scope.phone = '';
					}
				});
			}
		};

		return directive;
		//endregion
	}
})();