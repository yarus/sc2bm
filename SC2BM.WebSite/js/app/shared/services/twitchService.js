(function () {
    'use strict';

    angular.module('Sc2bmApp').service('twitchService', twitchService);

    twitchService.$inject = ['$http'];

    function twitchService($http) {
        function getVideo(videoUrl, callback) {
            var parts = videoUrl.split('/');
            var videoId = parts[parts.length - 2] + parts[parts.length - 1];
            var getUrl = "https://api.twitch.tv/kraken/videos/" + videoId;

            if (_.isFunction(callback)) {
                $http.get(getUrl).success(callback);
            } else {
                return $http.get(getUrl);
            }

            return null;
        }

        return {
            getVideo: getVideo
        };
    }
})();