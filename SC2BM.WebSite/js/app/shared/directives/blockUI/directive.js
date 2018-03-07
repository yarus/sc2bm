(function () {
	'use strict';

	angular.module('Sc2bmApp').directive('blockUi', blockUI);

	function blockUI() {
		//region Directive declaration
		var directive = {
			restrict: 'A',
			scope: {
				blockUiClass: '@blockUi'
			},
			require: ['blockUi', '?ngModel'],
			controller: ['$scope', '$compile', controller],
			link: link
		};

		return directive;
		//endregion

		//region Methods
		function controller($scope, $compile) {
			var self = this;
			var ngModelCtrl;
			var element;
			var elementPosition;

			this.init = function (ngModelCtrl_, element_) {
				ngModelCtrl = ngModelCtrl_;

				if (ngModelCtrl) {
					ngModelCtrl.$render = function () {
						self.render();
					};
				}
				else {
					$scope.$on('blockUI', function () {
						$scope.isBlocked = true;
						if ($scope.blockUiClass !== '') {
							self.setPosition();
						}
					});

					$scope.$on('unblockUI', function () {
						$scope.isBlocked = false;
						if ($scope.blockUiClass !== '') {
							self.setPosition();
						}
					});
				}

				if (_.isUndefined($scope.blockUiClass)) {
					$scope.blockUiClass = '';
				}

				element = element_;
				elementPosition = element.css('position');
			};

			this.render = function () {
				$scope.isBlocked = ngModelCtrl.$viewValue;
				if ($scope.blockUiClass !== '') {
					self.setPosition();
				}
			};

			this.setPosition = function () {
				element.css('position', $scope.isBlocked ? 'relative' : elementPosition);
			};

			this.compileBlock = function () {
				var blockUiClass = $scope.blockUiClass === '' ? '' : '-' + $scope.blockUiClass;
				var block = angular.element('<div data-ng-show="isBlocked" class="block-overlay' + blockUiClass + '"></div><div data-ng-show="isBlocked" class="block-loader' + blockUiClass + '"><i class="fa fa-spinner fa-pulse"></i></div>');
				element.append(block);
				$compile(block)($scope);
			};
		}

		function link(scope, element, attrs, ctrls) {
			var blockUiCtrl = ctrls[0], ngModelCtrl = ctrls[1];

			blockUiCtrl.init(ngModelCtrl, element);
			blockUiCtrl.compileBlock();
		}
		//endregion
	}
})();
