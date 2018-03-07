(function() {
	'use strict';

	angular.module('Sc2bmApp').directive('noteLink', noteLink);

	noteLink.$inject = ['$routeParams'];

	function noteLink($routeParams) {
		//region Directive declaration
		var directive = {
			restrict: 'E',
			transclude: true,
			replace: true,
			template: '<a href="{{noteLink}}"><ng-transclude></ng-transclude></a>',
			scope: {
				noteKey: '=',
				ngBind: '='
			},
			link: function (scope, element, attrs) {
				scope.$watch('noteKey', function (key) {
					if (key) {
						scope.noteLink = '/note/' + key;

						if ($routeParams.scopeName != null) {
							scope.noteLink += '/' + $routeParams.scopeName;
						}

						if ($routeParams.contextKey != null) {
							scope.noteLink += '/' + $routeParams.contextKey;
						}
					} else {
						scope.noteLink = '';
					}
				});
			}
		};

		return directive;
		//endregion
	}
})();