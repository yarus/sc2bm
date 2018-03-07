(function () {
    'use strict';

    angular.module('Sc2bmApp').service('rateRepository', rateRepository);

    rateRepository.$inject = ['$q', 'rateService'];

    function rateRepository($q, rateService) {
        var rates = [];

        return {
            reset: reset,
            getRate: getRate
        };

        //region Methods
        function reset() {
            rates = [];
        }

        function getRate(entityType, entityId) {
            var deferred = $q.defer();

            var item = _.find(rates, function (o) { return o.entityType == entityType && o.entityId == entityId; });
            if (item == null) {
                rateService.getTotalRate(entityType, entityId)
                    .success(function(result) {
                        if (result != null) {
                            var el = {};
                            el.value = parseFloat(result).toFixed(1);
                            el.entityType = entityType;
                            el.entityId = entityId;

                            rates.push(el);
                            deferred.resolve(el.value);
                        } else {
                            deferred.resolve(null);
                        }
                    })
                    .error (function(result) {
                        deferred.resolve(null);
                    });
            } else {
                deferred.resolve(item.value);
            }

            return deferred.promise;
        }
        //endregion
    }
})();