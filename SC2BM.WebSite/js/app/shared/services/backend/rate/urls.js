(function() {
    'use strict';

    angular.module('Sc2bmApp').constant('rateBackendUrls', {
        getPersonalRate: 'api/Rate/GetPersonalRate',
        getTotalRate: 'api/Rate/GetTotalRate',
        getRates: 'api/Rate/GetRates',
        addRate: 'api/Rate/AddRate',
        updateRate: 'api/Rate/UpdateRate',
        deleteRate: 'api/Rate/DeleteRate',
        getByID: 'api/Rate/GetByID'
    });
})();