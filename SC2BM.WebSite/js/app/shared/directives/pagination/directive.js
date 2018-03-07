(function () {
	'use strict';

	angular.module('Sc2bmApp').directive('pagination', pagination);

	pagination.$inject = ['$parse', 'paginationConfig'];

	function pagination($parse, paginationConfig) {
		//region Directive declaration
		var directive = {
			restrict: 'EA',
			scope: {
				totalItems: '=',
				firstText: '@',
				previousText: '@',
				nextText: '@',
				lastText: '@'
			},
			require: ['pagination', '?ngModel'],
			controller: 'paginationController',
			templateUrl: 'js/app/shared/directives/pagination/template.html',
			replace: true,
			link: link
		};

		return directive;
		//endregion

		//region Methods
		function link(scope, element, attrs, ctrls) {
			var paginationCtrl = ctrls[0], ngModelCtrl = ctrls[1];

			if (!ngModelCtrl) {
				return; // do nothing if no ng-model
			}

			// Setup configuration parameters
			var maxSize = angular.isDefined(attrs.maxSize) ? scope.$parent.$eval(attrs.maxSize) : paginationConfig.maxSize,
				rotate = angular.isDefined(attrs.rotate) ? scope.$parent.$eval(attrs.rotate) : paginationConfig.rotate;
			scope.boundaryLinks = angular.isDefined(attrs.boundaryLinks) ? scope.$parent.$eval(attrs.boundaryLinks) : paginationConfig.boundaryLinks;
			scope.directionLinks = angular.isDefined(attrs.directionLinks) ? scope.$parent.$eval(attrs.directionLinks) : paginationConfig.directionLinks;

			paginationCtrl.init(ngModelCtrl, paginationConfig);

			if (attrs.maxSize) {
				scope.$parent.$watch($parse(attrs.maxSize), function (value) {
					maxSize = parseInt(value, 10);
					paginationCtrl.render();
				});
			}

			// Create page object used in template
			function makePage(number, text, isActive) {
				return {
					number: number,
					text: text,
					active: isActive
				};
			}

			function getPages(currentPage, totalPages) {
				var pages = [];

				// Default page limits
				var startPage = 1, endPage = totalPages;
				var isMaxSized = (angular.isDefined(maxSize) && maxSize < totalPages);

				// recompute if maxSize
				if (isMaxSized) {
					if (rotate) {
						// Current page is displayed in the middle of the visible ones
						startPage = Math.max(currentPage - Math.floor(maxSize / 2), 1);
						endPage = startPage + maxSize - 1;

						// Adjust if limit is exceeded
						if (endPage > totalPages) {
							endPage = totalPages;
							startPage = endPage - maxSize + 1;
						}
					} else {
						// Visible pages are paginated with maxSize
						startPage = ((Math.ceil(currentPage / maxSize) - 1) * maxSize) + 1;

						// Adjust last page if limit is exceeded
						endPage = Math.min(startPage + maxSize - 1, totalPages);
					}
				}

				// Add page number links
				for (var number = startPage; number <= endPage; number++) {
					var page = makePage(number, number, number === currentPage);
					pages.push(page);
				}

				// Add links to move between page sets
				if (isMaxSized && !rotate) {
					if (startPage > 1) {
						var previousPageSet = makePage(startPage - 1, '...', false);
						pages.unshift(previousPageSet);
					}

					if (endPage < totalPages) {
						var nextPageSet = makePage(endPage + 1, '...', false);
						pages.push(nextPageSet);
					}
				}

				return pages;
			}

			var originalRender = paginationCtrl.render;
			paginationCtrl.render = function () {
				originalRender();
				if (scope.page > 0 && scope.page <= scope.totalPages) {
					scope.pages = getPages(scope.page, scope.totalPages);
				}
			};
		}
		//endregion
	}
})();
