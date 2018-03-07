(function () {
    'use strict';

    angular.module('Sc2bmApp').directive('emailLink', emailLink);

    emailLink.$inject = ['$routeParams'];

    function emailLink($routeParams) {
        //region Directive declaration
        var directive = {
            restrict: 'E',
            transclude: true,
            replace: true,
            template: '<a href="{{emailLink}}"><ng-transclude></ng-transclude></a>',
            scope: {
            	emailId: '=',
                ngBind: '='
            },
            link: function (scope, element, attrs) {
                scope.$watch('emailId', function (key) {
                    if (key) {
                        scope.emailLink = '/email/' + key;

                        if ($routeParams.scopeName != null) {
                            scope.emailLink += '/' + $routeParams.scopeName;
                        }

                        if ($routeParams.contextKey != null) {
                            scope.emailLink += '/' + $routeParams.contextKey;
                        }
                    } else {
                        scope.emailLink = '';
                    }
                });
            }
        };

        return directive;
        //endregion
    }
})();