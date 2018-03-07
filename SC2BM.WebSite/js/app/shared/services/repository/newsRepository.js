(function () {
    'use strict';

    angular.module('Sc2bmApp').service('newsRepository', newsRepository);

    newsRepository.$inject = ['$q', 'newsService'];

    function newsRepository($q, newsService) {
        var cacheExpirationMinutes = 30;

        var newsCache = {
            timeStamp: null,
            totalItems: 0,
            news: []
        };

        return {
            reset: reset,
            getLatestNews: getLatestNews
        };

        //region Methods
        function reset() {
            newsCache = {
                timeStamp: null,
                totalItems: 0,
                news: []
            };
        }

        function getLatestNews(count) {
            var deferred = $q.defer();

            var needServerLoading = true;

            if (newsCache.news.length > 0 && !_isCacheExpired() && _isEnoughCachedData(count)) {
                needServerLoading = false;
                deferred.resolve(newsCache.news);
            }

            if (needServerLoading) {
                var newsRequest = { pageNumber: 1, rowsPerPage: count, totalItems: 0, orderBy: "AddedDate", orderByAscending: false, filter: { IsDeleted: 0 } };
                newsService.searchNews(newsRequest)
                    .success(function (result) {
                        if (result != null && result.Items != null) {
                            newsCache.timeStamp = new Date();
                            newsCache.news = result.Items;
                            newsCache.totalItems = result.TotalCount;
                        }

                        deferred.resolve(newsCache.news);
                    })
                    .error(function (result) {
                        deferred.resolve(null);
                    });
            }

            return deferred.promise;
        }

        function _isEnoughCachedData(count) {
            return (newsCache.news.length >= count || (newsCache.news.length == newsCache.totalItems));
        }

        function _isCacheExpired() {
            var cacheExpired = true;

            if (newsCache.timeStamp != null) {
                var today = new Date();
                var diffMs = (today - newsCache.timeStamp);
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