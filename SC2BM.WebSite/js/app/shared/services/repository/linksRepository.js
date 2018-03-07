(function () {
    'use strict';

    angular.module('Sc2bmApp').service('linksRepository', linksRepository);

    linksRepository.$inject = ['$q', 'linkService'];

    function linksRepository($q, linkService) {
        var cacheExpirationMinutes = 30;

        var cache = {
            timeStamp: new Date(),
            totalItems: 0,
            items: [],
            latestVod: null
        };

        return {
            reset: reset,
            getLatestLinks: getLatestLinks,
            getLatestVod: getLatestVod
        };

        //region Methods
        function reset() {
            cache = {
                timeStamp: new Date(),
                totalItems: 0,
                items: [],
                latestVod: null
            };
        }

        function getLatestVod() {
            var deferred = $q.defer();

            var needServerLoading = true;

            if (cache.latestVod != null && !_isCacheExpired()) {
                needServerLoading = false;
                deferred.resolve(cache.latestVod);
            }

            if (needServerLoading) {
                linkService.getLatestVod()
                    .success(function (result) {
                        if (result != null) {
                            cache.latestVod = result;
                        }

                        deferred.resolve(cache.latestVod);
                    })
                    .error(function (result) {
                        deferred.resolve(null);
                    });
            }

            return deferred.promise;
        }

        function getLatestLinks(count) {
            var deferred = $q.defer();

            var needServerLoading = true;

            if (cache.items.length > 0 && !_isCacheExpired() && _isEnoughCachedData(count)) {
                needServerLoading = false;
                deferred.resolve(cache.items);
            }

            if (needServerLoading) {
                linkService.getLatestLinks('', count)
                    .success(function (results) {
                        if (results != null) {
                            cache.items = results;
                            cache.totalItems = results.length;
                        }

                        deferred.resolve(cache.items);
                    })
                    .error(function (result) {
                        deferred.resolve(null);
                    });
            }

            return deferred.promise;
        }

        function _isEnoughCachedData(count) {
            return (cache.items.length >= count || (cache.items.length == cache.totalItems));
        }

        function _isCacheExpired() {
            var cacheExpired = true;

            if (cache.timeStamp != null) {
                var today = new Date();
                var diffMs = (today - cache.timeStamp);
                var diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); // minutes
                if (diffMins < cacheExpirationMinutes) {
                    cacheExpired = false;
                }
            }

            return cacheExpired;
        }
        //endregion
    }
})();