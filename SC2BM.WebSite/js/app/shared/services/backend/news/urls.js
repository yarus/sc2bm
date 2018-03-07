(function() {
    'use strict';

    angular.module('Sc2bmApp').constant('newsBackendUrls', {
        searchNews: 'api/News/SearchNews',
        addOrUpdateNews: 'api/News/AddOrUpdateNews',
        deleteNews: 'api/News/DeleteNews'
    });
})();