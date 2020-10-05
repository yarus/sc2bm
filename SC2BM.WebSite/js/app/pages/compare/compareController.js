(function () {
    'use strict';

    angular.module('Sc2bmApp').controller('compareController', compareController);

    compareController.$inject = ['$routeParams', '$location', '$filter', 'buildOrderService', 'buildItemsImages'];

    function compareController($routeParams, $location, $filter, buildOrderService, buildItemsImages) {
        //region ViewModel declaration
        /*jshint validthis:true */
        var vm = this;
        vm.showWorkers = true;
        vm.leftBuildList = [];
        vm.rightBuildList = [];

        vm.showWorkersChanged = showWorkersChanged;
        vm.showStats = showStats;
        vm.versionChanged = versionChanged;
        vm.leftFactionChanged = leftFactionChanged;
        vm.rightFactionChanged = rightFactionChanged;
        vm.leftVsFactionChanged = leftVsFactionChanged;
        vm.rightVsFactionChanged = rightVsFactionChanged;
        vm.showComparison = showComparison;
        vm.closeComparison = closeComparison;
        vm.leftBuildSelected = leftBuildSelected;
        vm.rightBuildSelected = rightBuildSelected;

        vm.versions = ['All', 'Legacy of the Void', 'Heart of the Swarm', 'Wings of Liberty'];
        vm.factions = ['All', 'Terran', 'Protoss', 'Zerg'];
        vm.version = 'All';
        vm.leftRace = 'All';
        vm.rightRace = 'All';
        vm.leftVsRace = 'All';
        vm.rightVsRace = 'All';

        vm.showLeftBuildSelector = false;
        vm.showRightBuildSelector = false;
        vm.showCompareTable = false;
        vm.showSelectors = false;

        vm.dynamicPopover = {
            title: "Title",
            templateUrl: "templatePopover.html"
        };

        activate();
        //endregion

        //region Methods
        function activate() {
            vm.firstBuildId = $routeParams.firstBuildId;
            vm.secondBuildId = $routeParams.secondBuildId;

            vm.leftRequest = { pageNumber: 1, rowsPerPage: 1000, totalItems: 0, orderBy: "AddedDate", orderByAscending: false, filter: { IsDeleted: false } };
            vm.rightRequest = { pageNumber: 1, rowsPerPage: 1000, totalItems: 0, orderBy: "AddedDate", orderByAscending: false, filter: { IsDeleted: false } };

            if (vm.firstBuildId != null) {
                _loadBuildOrder(vm.firstBuildId, function(leftBuild) {
                    vm.leftName = leftBuild;

                    _setRequestsBasedOnLeftBuild();

                    _loadBuildList(vm.leftRequest, 0);

                    if (vm.secondBuildId != null) {
                        _loadBuildOrder(vm.secondBuildId, function (rightBuild) {
                            vm.rightName = rightBuild;

                            if (vm.rightName.SC2VersionID != vm.leftName.SC2VersionID) {
                                alert('Only builds from same version can be compared!');
                                $location.path("/home");
                                return;
                            }

                            _updateRequestsBasedOnRightBuild();

                            _loadBuildList(vm.rightRequest, 1);

                            vm.showSelectors = false;
                            _buildCompareModel();
                            vm.showCompareTable = true;
                        });
                    } else {
                        vm.showLeftBuildSelector = true;
                        vm.showRightBuildSelector = true;
                        vm.showSelectors = true;

                        _loadBuildList(vm.rightRequest, 1);
                    }
                });
            } else {
                vm.showLeftBuildSelector = true;
                vm.showRightBuildSelector = true;
                vm.showSelectors = true;

                _loadBuildList(vm.leftRequest, 0);
                _loadBuildList(vm.rightRequest, 1);
            }
        }

        function _setRequestsBasedOnLeftBuild() {
            vm.version = _getAddonByVersion(vm.leftName.SC2VersionID);
            vm.leftRace = vm.leftName.Race;
            vm.leftVsRace = vm.leftName.VsRace;
            vm.leftRequest.filter.Race = vm.leftName.Race;
            vm.leftRequest.filter.VsRace = vm.leftName.VsRace;
            vm.leftRequest.filter.SC2VersionID = vm.leftName.SC2VersionID;

            vm.rightRace = vm.leftName.VsRace;
            vm.rightVsRace = vm.leftName.Race;
            vm.rightRequest.filter.SC2VersionID = vm.leftName.SC2VersionID;
            vm.rightRequest.filter.Race = vm.leftName.VsRace;
            vm.rightRequest.filter.VsRace = vm.leftName.Race;
        }

        function _updateRequestsBasedOnRightBuild() {
            vm.rightRace = vm.rightName.Race;
            vm.rightVsRace = vm.rightName.VsRace;
            vm.rightRequest.filter.Race = vm.rightName.Race;
            vm.rightRequest.filter.VsRace = vm.rightName.VsRace;
            vm.rightRequest.filter.SC2VersionID = vm.rightName.SC2VersionID;
        }

        function leftBuildSelected(newBuild) {
            _loadBuildOrder(newBuild.ID, function(build) {
                vm.leftName = build;
                var item = _.find(vm.leftBuildList, function (o) { return o.Name == vm.leftName.Name; });
                var index = vm.leftBuildList.indexOf(item);
                vm.leftBuildList[index] = vm.leftName;
                _setRequestsBasedOnLeftBuild();
                _loadBuildList(vm.rightRequest, 1);
            });
        }

        function rightBuildSelected(newBuild) {
            _loadBuildOrder(newBuild.ID, function (build) {
                vm.rightName = build;
                var item = _.find(vm.rightBuildList, function (o) { return o.Name == vm.rightName.Name; });
                var index = vm.rightBuildList.indexOf(item);
                vm.rightBuildList[index] = vm.rightName;
                _updateRequestsBasedOnRightBuild();
            });
        }

        function _loadBuildOrder(buildId, callback) {
            buildOrderService.getBuildByID(buildId)
                .success(function(result) {
                    if (result != null && callback != null) {
                        result.createdDate = $filter('date')(result.AddedDate, 'fullDate');
                        result.matchup = result.Race[0] + "v" + result.VsRace[0];
                        result.addon = _getAddonByVersion(result.SC2VersionID);

                        var tmpResult = result;
                        buildOrderService.getBuildItemsForBuild(tmpResult.ID)
                            .success(function (results) {
                                if (results != null && results.length > 0) {
                                    tmpResult.loadedBuildItems = results;

                                    _setFilteredBuildItems(tmpResult);

                                    tmpResult.loadedBuildItems.forEach(function (element, index, array) {
                                        element.LogoPath = _getLogoByName(element.Name);
                                    });

                                    if (callback) {
                                        callback(tmpResult);
                                    }
                                }
                            });
                    }
                });
        }

        function closeComparison() {
            if (vm.firstBuildId == null) {
                vm.leftName = null;
                vm.rightName = null;

                vm.version = 'All';
                vm.leftRace = 'All';
                vm.rightRace = 'All';
                vm.leftVsRace = 'All';
                vm.rightVsRace = 'All';

                vm.leftRequest = { pageNumber: 1, rowsPerPage: 1000, totalItems: 0, orderBy: "AddedDate", orderByAscending: false, filter: { IsDeleted: false } };
                vm.rightRequest = { pageNumber: 1, rowsPerPage: 1000, totalItems: 0, orderBy: "AddedDate", orderByAscending: false, filter: { IsDeleted: false } };

                vm.showLeftBuildSelector = true;
                vm.showRightBuildSelector = true;
                vm.showCompareTable = false;
                vm.showSelectors = true;
                vm.compareModel = null;

                _loadBuildList(vm.leftRequest, 0);
                _loadBuildList(vm.rightRequest, 1);
            } else {
                $location.path("/compare");
            }
            
            return;
        }

        function showComparison() {
            $location.path("/compare/" + vm.leftName.ID + "/" + vm.rightName.ID);
            return;
        }

        function rightFactionChanged(newFaction) {
            if (newFaction == 'All') {
                vm.rightRequest.filter.Race = null;
            } else {
                vm.rightRequest.filter.Race = newFaction;
            }

            _loadBuildList(vm.rightRequest, 1);
        }

        function leftFactionChanged(newFaction) {
            if (newFaction == 'All') {
                vm.leftRequest.filter.Race = null;
            } else {
                vm.leftRequest.filter.Race = newFaction;
            }

            _loadBuildList(vm.leftRequest, 0);
        }

        function rightVsFactionChanged(newFaction) {
            if (newFaction == 'All') {
                vm.rightRequest.filter.VsRace = null;
            } else {
                vm.rightRequest.filter.VsRace = newFaction;
            }

            _loadBuildList(vm.rightRequest, 1);
        }

        function leftVsFactionChanged(newFaction) {
            if (newFaction == 'All') {
                vm.leftRequest.filter.VsRace = null;
            } else {
                vm.leftRequest.filter.VsRace = newFaction;
            }

            _loadBuildList(vm.leftRequest, 0);
        }

        function versionChanged(newVersion) {
            vm.leftName = null;
            vm.rightName = null;

            if (newVersion == 'Legacy of the Void') {
                vm.leftRequest.filter.SC2VersionID = '5.0.3';
                vm.rightRequest.filter.SC2VersionID = '5.0.3';
            } else if (newVersion == 'Heart of the Swarm') {
                vm.leftRequest.filter.SC2VersionID = '2.2.0';
                vm.rightRequest.filter.SC2VersionID = '2.2.0';
            } else if (newVersion == 'Wings of Liberty') {
                vm.leftRequest.filter.SC2VersionID = '2.0.5';
                vm.rightRequest.filter.SC2VersionID = '2.0.5';
            } else {
                vm.leftRequest.filter.SC2VersionID = null;
                vm.rightRequest.filter.SC2VersionID = null;
            }

            _loadBuildList(vm.leftRequest, 0);
            _loadBuildList(vm.rightRequest, 1);
        }

        function _loadBuildList(request, controlIndex) {
            var tmpRequest = request;
            buildOrderService.searchBuildOrders(request)
                .success(function (result) {
                    if (result != null) {
                        tmpRequest.totalItems = result.TotalCount;

                        if (controlIndex == 0) {
                            vm.leftBuildList = result.Items;
                            vm.leftBuildList.forEach(function (element, index, array) {
                                if (element.SC2VersionID != null) {
                                    element.SC2VersionName = _getAddonByVersion(element.SC2VersionID);
                                }
                            });
                            if (vm.leftName != null) {
                                var item = _.find(vm.leftBuildList, function (o) { return o.Name == vm.leftName.Name; });
                                var index = vm.leftBuildList.indexOf(item);
                                vm.leftBuildList[index] = vm.leftName;
                            }
                        } else {
                            vm.rightBuildList = result.Items;
                            vm.rightBuildList.forEach(function (element, index, array) {
                                if (element.SC2VersionID != null) {
                                    element.SC2VersionName = _getAddonByVersion(element.SC2VersionID);
                                }
                            });
                            if (vm.rightName != null) {
                                var rightItem = _.find(vm.rightBuildList, function (o) { return o.Name == vm.rightName.Name; });
                                var rightIndex = vm.rightBuildList.indexOf(rightItem);
                                vm.rightBuildList[rightIndex] = vm.rightName;
                            }
                        }
                    }
                });
        }

        function showWorkersChanged() {
            for (var i = 0; i < vm.builds.length; i++) {
                _setFilteredBuildItems(vm.builds[i]);
            }
            _buildCompareModel();
        }

        function showStats(compareItem, item, buildIndex) {
            var stats = item.AdditionalValues;

            vm.statsModel = {
                gas: stats.Gas,
                minerals: stats.Minerals,
                workers: stats.Drone || stats.SCV || stats.Probe,
                workersOnMinerals: stats.WorkersOnMinerals,
                workersOnGas: stats.WorkersOnGas,
                units: _calculateUnitsForSecond(vm.builds[buildIndex], compareItem.startedSecond, "unit"),
                buildings: _calculateUnitsForSecond(vm.builds[buildIndex], compareItem.startedSecond, "building"),
                upgrades: _calculateUnitsForSecond(vm.builds[buildIndex], compareItem.startedSecond, "upgrade"),
                workerLogoPath: _getWorkerLogoForBuild(vm.builds[buildIndex]),
                startTime: compareItem.startTime
            }
        }

        function _getWorkerLogoForBuild(build) {
            if (build.Race.toLowerCase() == "terran") {
                return "bi_scv.png";
            } else if (build.Race.toLowerCase() == "protoss") {
                return "bi_probe.png";
            } else if (build.Race.toLowerCase() == "zerg") {
                return "bi_drone.png";
            }

            return "empty_cell.png";
        }

        function _calculateUnitsForSecond(build, secondInTimeline, itemType) {
            var result = [];

            for (var i = 0; i < build.loadedBuildItems.length; i++) {
                var item = build.loadedBuildItems[i];

                if (item.Name.toLowerCase() == "scv" || item.Name.toLowerCase() == "probe" || item.Name.toLowerCase() == "drone") {
                    continue;
                }

                if (item.FinishedSecond <= secondInTimeline && item.ItemType.toLowerCase() == itemType.toLowerCase()) {
                    if (item.Name.toLowerCase().contains("onreactor")) {
                        var unit = item.Name.substring(0, item.Name.length - "onreactor".length);
                        var existingUnit = _findItemByName(unit, result);
                        if (existingUnit != null) {
                            existingUnit.count += 1;
                        }
                        continue;
                    }

                    if (item.Name.toLowerCase().contains("warpin")) {
                        var tosUnit = item.Name.substring("warpin".length, item.Name.length);
                        var existingTosUnit = _findItemByName(tosUnit, result);
                        if (existingTosUnit != null) {
                            existingTosUnit.count += 1;
                        } else {
                            var newTosItem = {
                                name: tosUnit,
                                logoPath: _getLogoByName(tosUnit),
                                count: 1
                            };
                            result.push(newTosItem);
                            continue;
                        }
                        continue;
                    }

                    var existingItem = _findItemByName(item.Name, result);
                    if (existingItem != null) {
                        existingItem.count += 1;
                    } else {
                        var newItem = {
                            name: item.Name,
                            logoPath: item.LogoPath,
                            count: 1
                        };
                        result.push(newItem);
                    }
                }
            }

            return result;
        }

        function _findItemByName(name, items) {
            for (var i = 0; i < items.length; i++) {
                if (items[i].name == name) {
                    return items[i];
                }
            }

            return null;
        }

        function _buildCompareModel() {
            var result = [];

            vm.builds = [];
            vm.builds.push(vm.leftName);
            vm.builds.push(vm.rightName);

            for (var i = 0; i < vm.builds.length; i++) {
                var build = vm.builds[i];

                for (var j = 0; j < build.buildItems.length; j++) {
                    var item = build.buildItems[j];

                    var existingItem = _findItemByTiming(item.StartedSecond, result);
                    if (existingItem != null) {
                        if (i == 0) {
                            existingItem.leftItems.push(item);
                        } else {
                            existingItem.rightItems.push(item);
                        }
                    } else {
                        var newItem = {
                            startedSecond: item.StartedSecond,
                            startTime: item.StartTime,
                            leftItems: [],
                            rightItems: []
                        };

                        if (i == 0) {
                            newItem.leftItems.push(item);
                        } else {
                            newItem.rightItems.push(item);
                        }
                        
                        result.push(newItem);
                    }
                }
            }

            result = _.sortBy(result, function (o) { return o.startedSecond; });

            vm.compareModel = result;
        }

        function _findItemByTiming(startSecond, items) {
            for (var i = 0; i < items.length; i++) {
                if (items[i].startedSecond == startSecond) {
                    return items[i];
                }
            }

            return null;
        }

        function _setFilteredBuildItems(buildRef) {
            if (vm.showWorkers) {
                buildRef.buildItems = buildRef.loadedBuildItems;
            } else {
                var filteredResults = [];
                for (var i = 0; i < buildRef.loadedBuildItems.length; i++) {
                    if (buildRef.loadedBuildItems[i].Name.toLowerCase() != "scv"
                        && buildRef.loadedBuildItems[i].Name.toLowerCase() != "probe"
                        && buildRef.loadedBuildItems[i].Name.toLowerCase() != "drone") {
                        filteredResults.push(buildRef.loadedBuildItems[i]);
                    }
                }
                buildRef.buildItems = filteredResults;
            }
        }

        function _getLogoByName(name) {
            for (var i = 0; i < buildItemsImages.length; i++) {
                if (buildItemsImages[i].name == name) {
                    return buildItemsImages[i].src + ".png";
                }
            }

            return "empty_cell.png";
        }

        function _getAddonByVersion(version) {
            var number = version.replace(/\./g, '');

            if (number <= 205) {
                return "Wings of Liberty";
            } else if (number > 205 && number <= 220) {
                return "Heart of the Swarm";
            } else if (number >= 300) {
                return "Legacy of the Void";
            }
        }
    }
})();