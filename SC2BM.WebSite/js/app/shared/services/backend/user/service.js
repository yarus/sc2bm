(function() {
    'use strict';

    angular.module('Sc2bmApp').service('userService', userService);

    userService.$inject = ['resource', 'userServiceUrls'];

    /**
     * @param resource
     * @param {noteBackendUrls} urls
     * @returns {*}
     */
    function userService(resource, urls) {
        return {
            getCountByUserName: getCountByUserName,
            getCountByEmail: getCountByEmail,
            registerUser: registerUser,
            confirmRegistration: confirmRegistration,
            getCurrentUser: getCurrentUser,
            getIp: getIp
        };

        //region Methods
        function getCountByUserName(userName) {
            return resource.get(urls.getCountByUserName, { userName: userName }, null, false);
        }

        function getCountByEmail(email) {
            return resource.get(urls.getCountByEmail, { email: email }, null, false);
        }

        function registerUser(user) {
            return resource.post(urls.register, { user: user });
        }

        function confirmRegistration(userName, confirmationSalt) {
            return resource.post(urls.confirmRegistration, { userName: userName, confirmationSalt: confirmationSalt });
        }

        function getCurrentUser() {
            return resource.get(urls.getCurrentUser, null, null, false);
        }

        function getIp() {
            return resource.get(urls.getIp, null, null, false);
        }
    	//endregion
    }
})();