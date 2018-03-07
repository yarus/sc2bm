(function () {
    'use strict';

    angular.module('Sc2bmApp').directive('mailtoLink', mailtoLink);

    function mailtoLink() {
        //region Directive declaration
        var directive = {
            restrict: 'E',
            replace: true,
            transclude: true,
            template: '<a href="mailto:{{address}}">{{address}}</a>',
            scope: {
                address: '='
            },
            link: function (scope, element, attrs) {
                scope.$watch('address', function (rawAddress) {
                    if (rawAddress) {
                        scope.address = rawAddress;
                    } else {
                        scope.address = '';
                    }
                });
            }
        };

        return directive;
        //endregion
    }
})();