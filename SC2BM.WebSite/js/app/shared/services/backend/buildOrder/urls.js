(function() {
    'use strict';

    angular.module('Sc2bmApp').constant('buildOrderBackendUrls', {
        searchBuildOrders: 'api/BuildOrder/SearchBuildOrders',
        addBuildOrder: 'api/BuildOrder/AddBuildOrder',
        getBuildByID: 'api/BuildOrder/GetByID',
        updateBuildOrder: 'api/BuildOrder/UpdateBuildOrder',
        deleteBuildOrder: 'api/BuildOrder/DeleteBuildOrder',
        getBuildItemsForBuild: 'api/BuildOrder/GetBuildItemsForBuild',
        getKbData: 'api/BuildOrder/GetKbData',
        getTopRatedBuildOrders: 'api/BuildOrder/GetTopRatedBuildOrders'
    });
})();