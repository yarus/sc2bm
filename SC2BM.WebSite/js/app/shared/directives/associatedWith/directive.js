(function() {
	'use strict';

	angular.module('Sc2bmApp').directive('associatedWith', associatedWith);

	associatedWith.$inject = ['$routeParams', 'session', 'entityService'];

	function associatedWith($routeParams, session, entityService) {
		//region controller function
		function controller($scope) {
		    if ($routeParams.contextKey && $routeParams.scopeName) {
		        entityService.getByKey($routeParams.scopeName, $routeParams.contextKey).success(function (result) {
		            $scope.entityType = $routeParams.scopeName;
					$scope.entity = result;
				});
			} else {
				$scope.memberKey = session.user.UserId;
				$scope.userName = session.user.FirstName + ' ' + session.user.LastName;
			}

			$scope.isEntityOfType = function (value) {
				return !_.isUndefined($scope.entityType) && $scope.entityType.toLowerCase() == value.toLowerCase();
			};
		}
		//endregion

		return {
			restrict: 'EA',
			scope: {},
			controller: ['$scope', controller],
			templateUrl: 'js/app/shared/directives/associatedWith/template.html'
		};
	}
})();