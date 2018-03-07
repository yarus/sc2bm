(function() {
    'use strict';

    angular.module('Sc2bmApp').constant('userServiceUrls', {
        getCountByUserName: 'api/User/GetCountByUserName',
        getCountByEmail: 'api/User/GetCountByEmail',
        register: 'api/User/Register',
        confirmRegistration: 'api/User/ConfirmRegistration',
        getCurrentUser: 'api/Authorization/GetCurrentUser',
        getIp: 'api/User/GetIp'
    });
})();