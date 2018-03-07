(function () {
    'use strict';

    angular.module('Sc2bmApp').directive('digitsOnly', digitsOnly);

    digitsOnly.$inject = ['helperService'];

    function digitsOnly(helperService) {
        return {
            require: '?ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                if (!ngModelCtrl) {
                    return;
                }

                ngModelCtrl.$parsers.push(function (val) {
                    if (angular.isUndefined(val)) {
                        return '';
                    }

                    var clean = helperService.formatDigit(val);

                    if (val !== clean) {
                        ngModelCtrl.$setViewValue(clean);
                        ngModelCtrl.$render();
                    }

                    return clean;
                });
            }
        };
    }
})();