(function () {
	'use strict';

	angular.module('Sc2bmApp').directive('multiSelectNode', multiSelectNode);

	multiSelectNode.$inject = ['RecursionHelper'];

	function multiSelectNode(RecursionHelper) {
		//region Directive declaration
		var directive = {
			restrict: 'AE',
			replace: true,
			templateUrl: 'js/app/shared/directives/multiSelect/node/multi-select-node.html',
			scope:
			{
				item: '='
			},
			link: function ($scope, element, attrs) {

			},
			controller: ['$scope', function ($scope) {
			    var selectInner = function (node, state) {
			        node.Checked = state;

			        _.forEach(node.Items, function (i) {
			            i.Checked = state;

			            if (i.Items && i.Items.length > 0) {
			                selectInner(i, state);
			            }
			        });
			    };

			    $scope.select = function (item, $event) {
			        if (item) {
			            selectInner(item, !item.Checked);
			            $event.preventDefault();
			        }
			    };

			    $scope.selectCheckbox = function (item, $event) {
			        if (item) {
			            selectInner(item, item.Checked);
			        }
			    };

			    $scope.toggle = function (item) {
			        item.Expanded = !item.Expanded;
			    };
			}],
			compile: function (element) {
				return RecursionHelper.compile(element, function (scope, iElement, iAttrs, controller, transcludeFn) {
					// Define your normal link function here.
					// Alternative: instead of passing a function,
					// you can also pass an object with 
					// a 'pre'- and 'post'-link function.
				});
			}
		};

		return directive;
		//endregion
	}
})();