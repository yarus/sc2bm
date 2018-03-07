(function () {
    'use strict';

    angular.module('Sc2bmApp').service('abbrService', abbrService);

    function abbrService() {
        function getAddonByVersion(version) {
            var number = version.replace(/\./g, '');

            if (number <= 205) {
                return "WOL";
            } else if (number > 205 && number <= 220) {
                return "HOTS";
            } else if (number >= 300) {
                return "LOTV";
            }

            return '';
        }

        function getRateClassByRate(rate) {
            var rateClass = '';

            if (rate >= 4) {
                rateClass = 'label-success';
            } else if (rate >= 2 && rate <= 3) {
                rateClass = 'label-warning';
            } else if (rate < 2) {
                rateClass = 'label-danger';
            }

            return rateClass;
        }

        function getMathupLabelForBuild(buildOrder) {
            var result = '';

            if (buildOrder != null && buildOrder.Race != null && buildOrder.VsRace != null) {
                return buildOrder.Race[0] + "v" + buildOrder.VsRace[0];
            }

            return result;
        }

        function getImageForMatchup(matchup) {
            if (matchup == null || matchup == '') {
                return '';
            }

            var matchupLower = matchup.toLowerCase();
            var validValues = ['tvt', 'tvz', 'tvp', 'pvp', 'pvz', 'pvt', 'zvz', 'zvt', 'zvp'];

            if (_.find(validValues, function(o) { return o == matchupLower; }) != null) {
                return "/images/factions/" + matchup + ".png";
            }

            return '';
        }

        function getAddonFullByVersion(version) {
            var number = version.replace(/\./g, '');

            if (number <= 205) {
                return "Wings of Liberty";
            } else if (number > 205 && number <= 220) {
                return "Heart of the Swarm";
            } else if (number >= 300) {
                return "Legacy of the Void";
            }

            return '';
        }

        function getFactionLogo(faction) {
            if (faction.toLowerCase() == "terran") {
                return "/images/factions/ic_terran_gradiented.png";
            } else if (faction.toLowerCase() == "protoss") {
                return '/images/factions/ic_protoss_gradiented.png';
            } else if (faction.toLowerCase() == "zerg") {
                return '/images/factions/ic_zerg_gradiented.png';
            }

            return '';
        }

        return {
            getAddonByVersion: getAddonByVersion,
            getRateClassByRate: getRateClassByRate,
            getMathupLabelForBuild: getMathupLabelForBuild,
            getImageForMatchup: getImageForMatchup,
            getAddonFullByVersion: getAddonFullByVersion,
            getFactionLogo: getFactionLogo
        };
    }
})();