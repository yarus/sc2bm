(function() {
    'use strict';

    angular.module('Sc2bmApp').constant('authBackendUrls', {
        logIn: 'api/Authorization/Login',
        logOff: 'api/Authorization/LogOff',
        getCurrentUser: 'api/Authorization/GetCurrentUser'
    });
})();