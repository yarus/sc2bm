(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('paginationController', paginationController);

	paginationController.$inject = ['$scope', '$attrs', '$parse'];

	function paginationController($scope, $attrs, $parse) {
		var self = this,
			ngModelCtrl = { $setViewValue: angular.noop }, // nullModelCtrl
			setNumPages = $attrs.numPages ? $parse($attrs.numPages).assign : angular.noop;

		this.init = function (nModelCtrl, config) {
			ngModelCtrl = nModelCtrl;
			this.config = config;

			ngModelCtrl.$render = function () {
				self.render();
			};

			if ($attrs.itemsPerPage) {
				$scope.$parent.$watch($parse($attrs.itemsPerPage), function (value) {
					self.itemsPerPage = parseInt(value, 10);
					$scope.totalPages = self.calculateTotalPages();
				});
			} else {
				this.itemsPerPage = config.itemsPerPage;
			}
		};

		this.calculateTotalPages = function () {
			var totalPages = this.itemsPerPage < 1 ? 1 : Math.ceil($scope.totalItems / this.itemsPerPage);
			return Math.max(totalPages || 0, 1);
		};

		this.render = function () {
			$scope.page = parseInt(ngModelCtrl.$viewValue, 10) || 1;
		};

		$scope.selectPage = function (page) {
			if ($scope.page !== page && page > 0 && page <= $scope.totalPages) {
				ngModelCtrl.$setViewValue(page);
				ngModelCtrl.$render();
			}
		};

		$scope.getText = function (key) {
			return $scope[key + 'Text'] || self.config[key + 'Text'];
		};
		$scope.noPrevious = function () {
			return $scope.page === 1;
		};
		$scope.noNext = function () {
			return $scope.page === $scope.totalPages;
		};

		$scope.$watch('totalItems', function () {
			$scope.totalPages = self.calculateTotalPages();
		});

		$scope.$watch('totalPages', function (value) {
			setNumPages($scope.$parent, value); // Readonly variable

			if ($scope.page > value) {
				$scope.selectPage(value);
			} else {
				ngModelCtrl.$render();
			}
		});
	}
})();
