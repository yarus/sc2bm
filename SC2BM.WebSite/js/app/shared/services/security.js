(function () {
    'use strict';

    angular.module('Sc2bmApp').service('security', security);

    security.$inject = ['$rootScope'];

    function security($rootScope) {
        var salt = 'rz8LuOtFBXphj9WQfvFh';
       
        function generate(userName, ip) {
            // Generates a token to be used for API calls. The first time during authentication, pass in a username/password. All subsequent calls can simply omit username and password, as the same token key (hashed password) will be used.

            // Set the key to a hash of the user's password + salt.
            var key = CryptoJS.enc.Base64.stringify(CryptoJS.HmacSHA256([ip, salt].join(':'), salt));

            // Get the (C# compatible) ticks to use as a timestamp. http://stackoverflow.com/a/7968483/2596404
            var ticks = ((new Date().getTime() * 10000) + 621355968000000000);

            // Construct the hash body by concatenating the username, ip, and userAgent.
            var message = [userName, ip, navigator.userAgent.replace(/ \.NET.+;/, '').replace(" SLCC2;", ""), ticks].join(':');

            // Hash the body, using the key.
            var hash = CryptoJS.HmacSHA256(message, key);

            // Base64-encode the hash to get the resulting token.
            var token = CryptoJS.enc.Base64.stringify(hash);

            // Include the username and timestamp on the end of the token, so the server can validate.
            var tokenId = [userName, ticks].join(':');

            // Base64-encode the final resulting token.
            var tokenStr = CryptoJS.enc.Utf8.parse([token, tokenId].join(':'));

            return CryptoJS.enc.Base64.stringify(tokenStr);
        }

        return {
            generate: generate
        };
    }
})();