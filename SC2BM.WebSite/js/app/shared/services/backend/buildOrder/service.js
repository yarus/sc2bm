(function() {
    'use strict';

    angular.module('Sc2bmApp').service('buildOrderService', buildOrderService);

    buildOrderService.$inject = ['resource', 'buildOrderBackendUrls'];

    function buildOrderService(resource, urls) {
        return {
            searchBuildOrders: searchBuildOrders,
            addBuildOrder: addBuildOrder,
            getBuildByID: getBuildByID,
            updateBuildOrder: updateBuildOrder,
            deleteBuildOrder: deleteBuildOrder,
            getBuildItemsForBuild: getBuildItemsForBuild,
            getKbData: getKbData,
            getTopRatedBuildOrders: getTopRatedBuildOrders
        };

        //region Methods
        function searchBuildOrders(request) {
            return resource.post(urls.searchBuildOrders, { request: request }, null, false);
        }

        function addBuildOrder(build) {
            return resource.post(urls.addBuildOrder, { build: build }, null, false);
        }

        function getBuildByID(id) {
            return resource.get(urls.getBuildByID, { id: id }, null, false);
        }

        function getBuildItemsForBuild(id) {
            return resource.get(urls.getBuildItemsForBuild, { id: id }, null, false);
        }

        function getKbData(versionId, faction) {
            return resource.get(urls.getKbData, { versionId: versionId, faction: faction }, null, false);
        }

        function getTopRatedBuildOrders(version, faction, vsFaction, count) {
            return resource.get(urls.getTopRatedBuildOrders, {version: version, faction: faction, vsFaction: vsFaction, count: count }, null, false);
        }

        function updateBuildOrder(build) {
            return resource.post(urls.updateBuildOrder, { build: build }, null, false);
        }

        function deleteBuildOrder(build) {
            return resource.post(urls.deleteBuildOrder, { build: build }, null, false);
        }
    	//endregion
    }
})();