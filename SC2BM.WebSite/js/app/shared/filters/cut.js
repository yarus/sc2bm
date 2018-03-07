(function() {
	'use strict';
	angular.module('Sc2bmApp').filter('cut', cutFilter);

	function cutFilter() {
		return function (input, limit, wordwiseOrSeparator, tail) {
			if (!input) {
				return '';
			}

			limit = parseInt(limit, 10);

			if (!limit) {
				return input;
			}

			var split = typeof wordwiseOrSeparator === 'string';
			var value = split ? input.split(wordwiseOrSeparator) : input;

			if (value.length <= limit) {
				return input;
			}

			value = value.slice(0, limit);

			if (_.isUndefined(wordwiseOrSeparator) || (typeof wordwiseOrSeparator === 'boolean' && wordwiseOrSeparator)) {
				var lastspace = value.lastIndexOf(' ');

				if (lastspace != -1) {
					value = value.slice(0, lastspace);
				}
			}
			else if (split) {
				value = value.join(wordwiseOrSeparator);
			}

			return value + (tail || ' …');
		};
	}
})();