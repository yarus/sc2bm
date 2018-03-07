(function() {
    'use strict';

    angular.module('Sc2bmApp').constant('linkBackendUrls', {
        getLinks: 'api/Link/GetLinks',
        addLink: 'api/Link/AddLink',
        updateLink: 'api/Link/UpdateLink',
        deleteLink: 'api/Link/DeleteLink',
        getByID: 'api/Link/GetByID',
        getLatestVod: 'api/Link/GetLatestVod',
        getLatestLinks: 'api/Link/GetLatestLinks',
        searchLinks: 'api/Link/SearchLinks'
    });
})();