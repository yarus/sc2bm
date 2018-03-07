(function() {
    'use strict';

    angular.module('Sc2bmApp').service('linkService', linkService);

    linkService.$inject = ['resource', 'linkBackendUrls'];

    function linkService(resource, urls) {
        return {
            getLinks: getLinks,
            addLink: addLink,
            getByID: getByID,
            updateLink: updateLink,
            deleteLink: deleteLink,
            getLatestVod: getLatestVod,
            getLatestLinks: getLatestLinks,
            searchLinks: searchLinks
        };

        //region Methods
        function getLinks(entityType, entityID) {
            return resource.get(urls.getLinks, { entityType: entityType, entityID: entityID }, null, false);
        }

        function getLatestLinks(linkType, amount) {
            return resource.get(urls.getLatestLinks, { linkType: linkType, amount: amount }, null, false);
        }

        function addLink(link) {
            return resource.post(urls.addLink, { link: link }, null, false);
        }

        function getByID(id) {
            return resource.get(urls.getByID, { id: id }, null, false);
        }

        function updateLink(link) {
            return resource.post(urls.updateLink, { link: link }, null, false);
        }

        function deleteLink(link) {
            return resource.post(urls.deleteLink, { link: link }, null, false);
        }

        function searchLinks(request) {
            return resource.post(urls.searchLinks, { request: request }, null, false);
        }

        function getLatestVod() {
            return resource.get(urls.getLatestVod, null, null, false);
        }
    	//endregion
    }
})();