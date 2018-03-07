(function() {
    'use strict';
    angular.module('Sc2bmApp').filter('offset', offsetFilter);

    function offsetFilter() {
        return function (input, start) {
            if (!input) {
                return null;
            }

            start = parseInt(start, 10);

            return input.slice(start);
        };
    }
})();