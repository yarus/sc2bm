(function() {
    'use strict';

    angular.module('Sc2bmApp').service('authService', authService);

    authService.$inject = ['resource', 'authBackendUrls'];

    function authService(resource, urls) {
        return {
            logIn: logIn,
            logOff: logOff,
            getCurrentUser: getCurrentUser
        };

        //region Methods
        function logIn(username, password, rememberMe) {
            return resource.post(urls.logIn, { username: username, password: password, rememberMe: rememberMe }, null, false);
        }

        function logOff(username) {
            return resource.post(urls.logOff, { username: username }, null, false);
        }

        function getCurrentUser() {
            return resource.get(urls.getCurrentUser, null, null, false);
        }
    	//endregion
    }
})();