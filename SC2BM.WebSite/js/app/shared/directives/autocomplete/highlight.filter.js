(function() {
	'use strict';

	angular.module('Sc2bmApp').filter('highlight', highlightFilter);

	highlightFilter.$inject = ['$sce'];

	function highlightFilter($sce) {
	    function escapeRegExp(str) {
	        return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&");
	    }

		return function (input, searchParam) {
			if (typeof input === 'function') {
				return '';
			}

			if (searchParam) {
				 var words = '(' +
						searchParam.split(/\ /).join(' |') + '|' +
						searchParam.split(/\ /).join('|') +
						')',
					exp = new RegExp(escapeRegExp(words), 'gi');
				if (words.length) {
					input = input.replace(exp, "<span class=\"highlight\">$1</span>");
				}
			}

			return $sce.trustAsHtml(input);
		};
	}
})();