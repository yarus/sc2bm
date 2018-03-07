(function() {
	'use strict';

	angular.module('Sc2bmApp').service('session', session);

	session.$inject = ['$rootScope', '$cookies', 'security'];

	function session($rootScope, $cookies, security) {
	    var ip = null;
	    var accessToken = null;
	    var user = null;

	    function _refreshAccessToken() {
	        var userTmp = getUser();
	        var ip = getIp();

            if (userTmp == null || ip == null) {
                return;
            }

            accessToken = security.generate(userTmp.UserName, ip);
            $cookies.token = accessToken;
	    }

        function getAccessToken() {
            if (accessToken == null) {
                accessToken = $cookies.token;

                if (accessToken == null || accessToken == "null") {
                    _refreshAccessToken();
                }
            }
            return accessToken;
        }

        function setIp(ipAddr) {
            ip = ipAddr;
            $cookies.ip = ip;
            _refreshAccessToken();
        }

        function getIp() {
            if (ip == null) {
                ip = $cookies.ip;
            }
            return ip;
        }

        function setUser(userParam) {
            user = userParam;
            $cookies.user = user;
            _refreshAccessToken();
        }

        function getUser() {
            if (user == null) {
                user = $cookies.user;
            }
            return user;
        }

        function isCurrentUserAdmin() {
            var user = getUser();
            return user != null && user.ID != null && user.Roles != null && user.Roles.length > 0 && user.Roles.indexOf("Admin") != -1;
        }

        function isUserSignedIn() {
            var user = getUser();
            return user != null && user.ID != null;
        }

        return {
            getAccessToken: getAccessToken,
            setIp: setIp,
            getIp: getIp,
            setUser: setUser,
	        getUser: getUser,
	        isCurrentUserAdmin: isCurrentUserAdmin,
	        isUserSignedIn: isUserSignedIn
	    };
	}
})();