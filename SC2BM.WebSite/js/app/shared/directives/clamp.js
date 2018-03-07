(function () {
	'use strict';

	angular.module('Sc2bmApp').directive('clamp', clamp);

	clamp.$inject = ['$timeout'];

	function clamp($timeout) {
		//region Directive declaration
		var directive = {
			restrict: 'A',
			scope: {
				clamp: '='
			},
			link: function (scope, element) {
				$timeout(function () {
					var options = {
						clamp: scope.clamp || 1,
						splitOnChars: [' ' ,'.', '-', '–', '—', ',', ';']
					};
					var originalText = element[0].textContent;
					$clamp(element[0], options);
					if (originalText != element[0].textContent) {
						element[0].title = originalText;
					}
				});
			}
		};

		return directive;
		//endregion
	}
})();