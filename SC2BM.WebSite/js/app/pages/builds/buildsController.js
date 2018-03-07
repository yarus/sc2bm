(function () {
    'use strict';

    angular.module('Sc2bmApp').controller('buildsController', buildsController);

    buildsController.$inject = ['$location', '$timeout', '$rootScope', '$filter', '$routeParams', 'abbrService', 'Upload', 'rateRepository', 'commentRepository', 'rateService', 'session', 'buildOrderService', 'commentService', 'linkService'];

    function buildsController($location, $timeout, $rootScope, $filter, $routeParams, abbrService, Upload, rateRepository, commentRepository, rateService, session, buildOrderService, commentService, linkService) {
        //region ViewModel declaration
        /*jshint validthis:true */
        var vm = this;

        vm.topRatedBuilds = [];
        vm.latestBuilds = [];
        vm.latestVods = [];
        vm.latestReplays = [];
        vm.latestLinks = [];
        vm.latestComments = [];

        vm.versions = ['All', 'Legacy of the Void', 'Heart of the Swarm', 'Wings of Liberty'];

        vm.version = 'All';
        vm.race = 'All';
        vm.vsRace = 'All';

        vm.faction = '';
        vm.vsFaction = '';
        vm.SC2VersionID = '';
        vm.searchName = '';

        vm.versionChanged = versionChanged;
        vm.factionChanged = factionChanged;
        vm.vsFactionChanged = vsFactionChanged;
        vm.loadMoreBuilds = loadMoreBuilds;
        vm.nameChanged = nameChanged;

        vm.pathTo = pathTo;
        vm.addToCompare = addToCompare;

        vm.leftCompare = null;
        vm.rightCompare = null;

        var latestBuildsToLoad = 20;

        activate();
        //endregion

        //region Methods
        function activate() {
            vm.enableUpload = session.isUserSignedIn();

            _handleRouteParams();

            _loadData();
        }

        function _handleRouteParams() {
            vm.version = 'All';
            if ($routeParams.version != null) {
                if ($routeParams.version.toLowerCase() == 'lotv') {
                    vm.version = vm.versions[1];
                } else if ($routeParams.version.toLowerCase() == 'hots') {
                    vm.version = vm.versions[2];
                } else if ($routeParams.version.toLowerCase() == 'wol') {
                    vm.version = vm.versions[3];
                }
            }

            _setVersionBasedOnName();

            if ($routeParams.race != null) {
                if ($routeParams.race.toLowerCase() == "terran") {
                    vm.faction = 'Terran';
                } else if ($routeParams.race.toLowerCase() == "protoss") {
                    vm.faction = 'Protoss';
                } else if ($routeParams.race.toLowerCase() == "zerg") {
                    vm.faction = 'Zerg';
                }
            }

            if ($routeParams.vsRace != null) {
                if ($routeParams.vsRace.toLowerCase() == "terran") {
                    vm.vsFaction = 'Terran';
                } else if ($routeParams.vsRace.toLowerCase() == "protoss") {
                    vm.vsFaction = 'Protoss';
                } else if ($routeParams.vsRace.toLowerCase() == "zerg") {
                    vm.vsFaction = 'Zerg';
                }
            }
        }

        function pathTo(href) {
            $location.path(href);
        }

        function addToCompare(build) {
            if (vm.leftCompare == null) {
                vm.leftCompare = build;

                if (vm.latestBuildsTotal == 1) {
                    var path = '/compare/' + vm.leftCompare.ID;
                    pathTo(path);
                }
            } else if (vm.leftCompare == build) {
                vm.leftCompare = null;
            } else if (vm.rightCompare == null) {
                vm.rightCompare = build;

                var path = '/compare/' + vm.leftCompare.ID + '/' + vm.rightCompare.ID;
                pathTo(path);
            }
        }

        function _loadData() {
            _loadTopRatedBuilds();
            _loadLatestBuilds();
            _loadLatestVods();
            _loadLatestReplays();
            _loadLatestLinks();
            _loadLatestComments();
        }

        function loadMoreBuilds() {
            latestBuildsToLoad += 10;

            if (latestBuildsToLoad > vm.latestBuildsTotal) {
                latestBuildsToLoad = vm.latestBuildsTotal;
            }

            _loadLatestBuilds();
        }

        function nameChanged() {
            _loadLatestBuilds();
        }

        function _setVersionBasedOnName() {
            if (vm.version == 'Legacy of the Void') {
                vm.SC2VersionID = '4.1.2';
            } else if (vm.version == 'Heart of the Swarm') {
                vm.SC2VersionID = '2.2.0';
            } else if (vm.version == 'Wings of Liberty') {
                vm.SC2VersionID = '2.0.5';
            } else {
                vm.SC2VersionID = '';
            }
        }

        function versionChanged() {
            pathTo(_getStatePath());
        }

        function _getStatePath() {
            var path = '/library';

            var version = 'all';
            if(vm.version == vm.versions[1]) {
                version = 'lotv';
            } else if (vm.version == vm.versions[2]) {
                version = 'hots';
            } else if (vm.version == vm.versions[3]) {
                version = 'wol';
            }

            path += '/' +version;

            if (vm.faction != '') {
                path += '/' + vm.faction.toLowerCase();
            }

            if (vm.vsFaction != '' && vm.faction != '') {
                path += '/vs/' + vm.vsFaction.toLowerCase();
            }

            return path;
        }

        function factionChanged(newFaction) {
            if (newFaction == vm.faction) {
                vm.faction = '';
            } else {
                vm.faction = newFaction;
            }

            pathTo(_getStatePath());
        }

        function vsFactionChanged(newFaction) {
            if (newFaction == vm.vsFaction) {
                vm.vsFaction = '';
            } else {
                vm.vsFaction = newFaction;
            }

            pathTo(_getStatePath());
        }

        function _loadTopRatedBuilds() {
            buildOrderService.getTopRatedBuildOrders(vm.SC2VersionID, vm.faction, vm.vsFaction, 4)
                .success(function (result) {
                    if (result != null && result.length > 0) {
                        vm.topRatedBuilds = result;

                        vm.topRatedBuilds.forEach(function (element, index, array) {
                            var el = element;
                            _loadBuildAdditionalData(el);
                        });
                    }
                });
        }

        function _loadLatestBuilds() {
            var request = { pageNumber: 1, rowsPerPage: latestBuildsToLoad, totalItems: 0, orderBy: "AddedDate", orderByAscending: false, filter: { IsDeleted: 0 } };

            if (vm.SC2VersionID != '') {
                request.filter.SC2VersionID = vm.SC2VersionID;
            } else {
                request.filter.SC2VersionID = null;
            }

            if (vm.faction != '') {
                request.filter.Race = vm.faction;
            } else {
                request.filter.Race = null;
            }

            if (vm.vsFaction != '') {
                request.filter.VsRace = vm.vsFaction;
            } else {
                request.filter.VsRace = null;
            }

            if (vm.searchName != null && vm.searchName != '') {
                request.filter.Name = vm.searchName;
            } else {
                request.filter.Name = null;
            }

            buildOrderService.searchBuildOrders(request)
                .success(function (result) {
                    if (result != null && result.Items != null) {
                        vm.latestBuilds = result.Items;
                        vm.latestBuildsTotal = result.TotalCount;

                        vm.latestBuilds.forEach(function (element, index, array) {
                            var el = element;
                            _loadBuildAdditionalData(el);
                        });
                    }
                });
        }

        function _loadLatestVods() {
            vm.latestVods = [];

            linkService.getLatestLinks('vod', 5)
                .success(function (result) {
                    if (result != null && result.length > 0) {
                        vm.latestVods = result;
                    }
                });
        }

        function _loadLatestReplays() {
            vm.latestReplays = [];

            linkService.getLatestLinks('replay', 5)
                .success(function (result) {
                    if (result != null && result.length > 0) {
                        vm.latestReplays = result;
                    }
                });
        }

        function _loadLatestLinks() {
            vm.latestLinks = [];

            linkService.getLatestLinks('href', 5)
                .success(function (result) {
                    if (result != null && result.length > 0) {
                        vm.latestLinks = result;
                    }
                });
        }

        function _loadLatestComments() {
            vm.latestComments = [];

            commentService.getLatestComments('Builds')
                .success(function (result) {
                    if (result != null && result.Items != null) {
                        vm.latestComments = result.Items;

                        vm.latestComments.forEach(function (element, index, array) {
                            if (element.Text != null) {
                                element.Text = $filter('htmlToTextWithLines')(element.Text);

                                if (element.Text.length > 100) {
                                    element.Text = element.Text.substring(0, 100).replace(/\n/g, '<br/>') + "...";
                                } else {
                                    element.Text = element.Text.replace(/\n/g, '<br/>');
                                }
                            }

                            element.EntityLink = "/builds/" + element.EntityID;
                        });
                    }
                });
        }

        function _loadBuildAdditionalData(build) {
            var el = build;

            rateRepository.getRate('Build', el.ID)
                .then(function(result) {
                    if (result != null) {
                        el.rate = parseFloat(result).toFixed(1);
                        el.rateClass = abbrService.getRateClassByRate(el.rate);
                    }
                });

            commentRepository.getTotalComments('Builds', el.ID)
                .then(function(result) {
                    if (result != null) {
                        el.commentsTotal = result;
                    }
                });

            if (el.Description != null) {
                el.Description = $filter('htmlToPlainText') (el.Description);

                if (el.Description.length > 300) {
                    el.Description = el.Description.substring(0, 300) + "...";
                }
            }

            el.matchup = abbrService.getMathupLabelForBuild(el);
            el.matchupLogo = abbrService.getImageForMatchup(el.matchup);
            el.factionLogo = abbrService.getFactionLogo(el.Race);

            var user = session.getUser();
            el.isOwner = session.isCurrentUserAdmin() || (session.isUserSignedIn() && user.ID == el.OwnerUserID);

            if (el.SC2VersionID != null) {
                el.SC2VersionName = abbrService.getAddonFullByVersion(el.SC2VersionID);
                el.addonAbbr = abbrService.getAddonByVersion(el.SC2VersionID);
            }

            el.microLabel =(el.MicroRate / 5) * 100 + "%";
            el.microStyle = {width: el.microLabel};
            el.macroLabel =(el.MacroRate / 5) * 100 + "%";
            el.macroStyle = {width: el.macroLabel};
            el.aggressionLabel = (el.AggressionRate / 5) * 100 + "%";
            el.aggressionStyle = { width: el.aggressionLabel };
            el.defenseLabel = (el.DefenceRate / 5) * 100 + "%";
            el.defenseStyle = {width: el.defenseLabel};
        }
    }
})();