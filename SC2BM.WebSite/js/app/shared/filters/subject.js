(function() {
	'use strict';
	angular.module('Sc2bmApp').filter('subject', subjectFilter);

	function subjectFilter() {
		return function (input) {
			return input || '<<No Subject>>';
		};
	}
})();