(function () {
	'use strict';

	angular.module('Sc2bmApp').constant('paginationConfig', {
		itemsPerPage: 10,
		maxSize: 5,
		boundaryLinks: true,
		directionLinks: true,
		firstText: '<<',
		previousText: '<',
		nextText: '>',
		lastText: '>>',
		rotate: true
	});
})();
