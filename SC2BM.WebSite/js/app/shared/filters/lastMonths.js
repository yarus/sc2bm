(function() {
	'use strict';
	angular.module('Sc2bmApp').filter('lastMonths', lastMonthFilter);

	function lastMonthFilter() {
		return function (items, field, months) {
			if (months) {
				var date = new Date();

				date.setMonth(date.getMonth() - months);

				return items.filter(function (item) {
					return (new Date(item[field]) >= date);
				});
			} else {
				return items;
			}
		};
	}
})();