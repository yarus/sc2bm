(function() {
    'use strict';

    angular.module('Sc2bmApp').service('rateService', rateService);

    rateService.$inject = ['resource', 'rateBackendUrls'];

    function rateService(resource, urls) {
        return {
            getPersonalRate: getPersonalRate,
            getTotalRate: getTotalRate,
            getRates: getRates,
            addRate: addRate,
            getByID: getByID,
            updateRate: updateRate,
            deleteRate: deleteRate
        };

        //region Methods
        function getRates(entityType, entityID) {
            return resource.get(urls.getRates, { entityType: entityType, entityID: entityID }, null, false);
        }

        function getTotalRate(entityType, entityID) {
            return resource.get(urls.getTotalRate, { entityType: entityType, entityID: entityID }, null, false);
        }

        function getPersonalRate(entityType, entityID, ownerUserID) {
            return resource.get(urls.getPersonalRate, { entityType: entityType, entityID: entityID, ownerUserID: ownerUserID }, null, false);
        }

        function addRate(rate) {
            return resource.post(urls.addRate, { rate: rate }, null, false);
        }

        function getByID(id) {
            return resource.get(urls.getByID, { id: id }, null, false);
        }

        function updateRate(rate) {
            return resource.post(urls.updateRate, { rate: rate }, null, false);
        }

        function deleteRate(rate) {
            return resource.post(urls.deleteRate, { rate: rate }, null, false);
        }
    	//endregion
    }
})();