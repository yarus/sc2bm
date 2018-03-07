(function () {
	'use strict';

	angular.module('Sc2bmApp').directive('comments', commentsDirective);

	commentsDirective.$inject = [];

	function commentsDirective() {
		//region Directive declaration
		var directive = {
			restrict: 'EA',
			scope: {
			    user: '=',
				entity: '=',
                type: '@',
			    count: '='
			},
			require: ['comments'],
			controller: 'commentsController',
			controllerAs: "vm",
			templateUrl: 'js/app/shared/directives/comments/template.html',
			replace: true
		};

		return directive;
		//endregion
	}
})();
