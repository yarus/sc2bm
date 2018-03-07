(function() {
	'use strict';
	angular.module('Sc2bmApp').filter("notEmptyFilter", notEmptyFilter);

	notEmptyFilter.$inject = ['$filter'];

	function notEmptyFilter($filter) {
		return function(array, params) {
			if (params) {
				return $filter('filter')(array, params);
			} else {
				return [];
			}
		};
	}
})();