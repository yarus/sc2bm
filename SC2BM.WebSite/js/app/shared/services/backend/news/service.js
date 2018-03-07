(function() {
    'use strict';

    angular.module('Sc2bmApp').service('newsService', newsService);

    newsService.$inject = ['resource', 'newsBackendUrls'];

    function newsService(resource, urls) {
        return {
            searchNews: searchNews,
            addOrUpdateNews: addOrUpdateNews,
            deleteNews: deleteNews
        };

        //region Methods
        function searchNews(request) {
            return resource.post(urls.searchNews, { request: request }, null, false);
        }

        function addOrUpdateNews(item) {
            return resource.post(urls.addOrUpdateNews, { item: item }, null, false);
        }

        function deleteNews(item) {
            return resource.post(urls.deleteNews, { item: item }, null, false);
        }
    	//endregion
    }
})();