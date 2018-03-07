(function() {
	'use strict';
	angular.module('Sc2bmApp').filter('cta', ctaFilter);

	ctaFilter.$inject = ['$filter'];

	function ctaFilter($filter) {
		return function (input, decimals) {
			var value = $filter('number')(input, decimals);
			return input < 0 ? '(' + value + ')' : value;
		};
	}
})();