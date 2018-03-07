(function () {
	'use strict';

	angular.module('Sc2bmApp').directive('multiSelect', multiSelect);

	multiSelect.$inject = ['$document'];

	function multiSelect($document) {
		//region Directive declaration
		var directive = {
			restrict: 'AE',
			replace: true,
			templateUrl: 'js/app/shared/directives/multiSelect/multi-select.html',
			scope:
			{
				// models
				inputModel: '=',
				outputModel: '=',
				defaultLabel: '@',
				itemLabel: '@'
			},
			link: function ($scope, element, attrs) {
				$scope.varButtonLabel = $scope.defaultLabel;
				$scope.expanded = false;

				$scope.selectAll = selectAll;
				$scope.toggle = toggle;

				activate();

				function activate() {
					$scope.$watch('inputModel', _onInputModelChanged, true);
				}

				function selectAll(state) {
					_.forEach($scope.inputModel, function (item) {
						_selectInternal(item, state);
					});
				}

				function toggle() {
					$scope.expanded = !$scope.expanded;

					if ($scope.expanded) {
						$document.one('click', _onDocumentClick);
					}
				}

				function _selectInternal(item, state) {
					item.Checked = state;

					_.forEach(item.Items, function (i) {
						_selectInternal(i, state);
					});
				}

				function _onInputModelChanged(val) {
					if (val) {
						var result = [];
						_.forEach(val, function(item) {
							_findInternal(item, result);
						});

						$scope.outputModel = result;
						if (result.length > 0) {
							$scope.varButtonLabel = result.length + ' selected items';
							$scope.isSelected = true;
						} else {
							$scope.varButtonLabel = $scope.defaultLabel;
							$scope.isSelected = false;
						}
					}
				}

				function _findInternal(item, result) {
					if (item.Value && item.Checked) {
						result.push(item);
					}

					if (item.Items) {
						_.forEach(item.Items, function(i) {
							_findInternal(i, result);
						});
					}
				}

				function _onDocumentClick(event) {
					var isClickedElementChildOfPopup = element
							.find(event.target)
							.length > 0;

					if (isClickedElementChildOfPopup) {
						$document.one('click', _onDocumentClick);
						return;
					}

					if ($scope.expanded) {
						$scope.expanded = false;
						$scope.$apply();
					} else {
						$document.one('click', _onDocumentClick);
					}
				}
			}
		};

		return directive;

		//endregion
	}
})();