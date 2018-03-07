(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('buildViewController', buildViewController);

	buildViewController.$inject = ['$routeParams', '$location', '$filter', 'session', 'buildOrderService', 'commentService', 'rateService', 'linkService', 'buildItemsImages'];

	function buildViewController($routeParams, $location, $filter, session, buildOrderService, commentService, rateService, linkService, buildItemsImages) {
		//region ViewModel declaration
		/*jshint validthis:true */
	    var vm = this;
	    vm.user = {};

	    vm.isOwner = false;
	    vm.deleteBuildOrder = deleteBuildOrder;
	    vm.buildOrder = null;

	    vm.commentsModel = {};

	    vm.isRated = false;
	    vm.rate = 0;
	    vm.addRate = addRate;
	    vm.deleteRate = deleteRate;
	    vm.deleteLink = deleteLink;
	    vm.showStats = showStats;

	    vm.showWorkers = true;
	    vm.showWorkersChanged = showWorkersChanged;

	    vm.vods = [];
	    vm.replays = [];
	    vm.hrefs = [];

	    vm.buildItems = [];
	    vm.loadedBuildItems = [];

	    vm.dynamicPopover = {
	        title: "Title",
	        templateUrl: "templatePopover.html"
	    };

		activate();
		//endregion

		//region Methods
		function activate() {
		    vm.user = session.getUser();
		    vm.isAdmin = session.isCurrentUserAdmin();

		    vm.buildID = $routeParams.id;

		    if (vm.buildID == null) {
                $location.path("/home");
                return;
            }

		    _loadBuildOrder();
		    _loadTotalRate();
		    _loadPersonalRate();
		    _loadLinks();
		}

		function showStats(item) {
		    var stats = item.AdditionalValues;

		    vm.statsModel = {
		        gas: stats.Gas,
		        minerals: stats.Minerals,
		        workers: stats.Drone || stats.SCV || stats.Probe,
		        workersOnMinerals: stats.WorkersOnMinerals,
		        workersOnGas: stats.WorkersOnGas,
		        units: _calculateUnitsForSecond(item.StartedSecond, "unit"),
		        buildings: _calculateUnitsForSecond(item.StartedSecond, "building"),
		        upgrades: _calculateUnitsForSecond(item.StartedSecond, "upgrade"),
		        workerLogoPath: _getWorkerLogoForBuild(vm.buildOrder),
		        startTime: item.StartTime
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

		function _calculateUnitsForSecond(secondInTimeline, itemType) {
		    var result = [];

		    for (var i = 0; i < vm.loadedBuildItems.length; i++) {
		        var item = vm.loadedBuildItems[i];

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

		function _loadLinks() {
            linkService.getLinks('Build', vm.buildID)
                .success(function (result) {
                    vm.vods = [];
                    vm.replays = [];
                    vm.hrefs = [];

                    result.Items.forEach(function (element, index, array) {
                        if (element.Type == "vod") {
                            vm.vods.push(element);
                        } else if (element.Type == "replay") {
                            vm.replays.push(element);
                        } else if (element.Type == "href") {
                            vm.hrefs.push(element);
                        }
                    });
                });
        }

		function _loadPersonalRate() {
            if (vm.user == null || vm.user.ID == null) {
                return;
            }

		    rateService.getPersonalRate('Build', vm.buildID, vm.user.ID)
		        .success(function (result) {
		            if (result.TotalCount > 0) {
                        vm.personalRate = result.Items[0];
                    } else {
		                vm.personalRate = null;
		            }
		        });
		}

		function deleteRate() {
		    if (vm.personalRate == null) {
		        return;
		    }

		    rateService.deleteRate(vm.personalRate)
		        .success(function(result) {
		            vm.personalRate = null;
		            _loadTotalRate();
		        });
		}

		function deleteLink(link) {
		    if (link == null || link.ID == null) {
		        return;
		    }

		    linkService.deleteLink(link)
		        .success(function(result) {
		            _loadLinks();
		        });
		}

        function _loadTotalRate() {
            rateService.getTotalRate('Build', vm.buildID)
                .success(function (result) {
                    if (result != null && result != '') {
                        vm.rate = parseFloat(result).toFixed(1);
                    } else {
                        vm.rate = 0;
                    }
                });
        }

        function addRate(value) {
            var rate = {OwnerUserID: vm.user.ID, EntityID: vm.buildID, EntityType: 'Build', Value: value};

            if (vm.personalRate == null) {
                rateService.addRate(rate)
                    .success(function() {
                        _loadTotalRate();
                        _loadPersonalRate();
                    });
            } else {
                rate.ID = vm.personalRate.ID;

                rateService.updateRate(rate)
                    .success(function () {
                        _loadTotalRate();
                        _loadPersonalRate();
                    });
            }
        }

		function _loadBuildOrder() {
		    buildOrderService.getBuildByID(vm.buildID)
                .success(function (result) {
                    vm.buildOrder = result;

                    //vm.buildItems = vm.buildOrder.BuildItems.join();
                    vm.createdDate = $filter('date')(vm.buildOrder.AddedDate, 'fullDate');
                   
		            vm.matchup = getImageForMatchup(vm.buildOrder.Race[0] + "v" + vm.buildOrder.VsRace[0]);
		            vm.isOwner = (vm.user != null && vm.buildOrder.OwnerUserID == vm.user.ID) || session.isCurrentUserAdmin();
		            vm.addon = getAddonByVersion(vm.buildOrder.SC2VersionID);

		            vm.microLabel = (vm.buildOrder.MicroRate / 5) * 100 + "%";
		            vm.microStyle = { width: vm.microLabel };
		            vm.macroLabel = (vm.buildOrder.MacroRate / 5) * 100 + "%";
		            vm.macroStyle = { width: vm.macroLabel };
		            vm.aggressionLabel = (vm.buildOrder.AggressionRate / 5) * 100 + "%";
		            vm.aggressionStyle = { width: vm.aggressionLabel };
		            vm.defenseLabel = (vm.buildOrder.DefenceRate / 5) * 100 + "%";
		            vm.defenseStyle = { width: vm.defenseLabel };

		            _loadBuildItems(vm.buildOrder.ID);
		        })
                .error(function (result) {
                    var errorMsg = "Error while loading build.";

                    if (result != null) {
                        errorMsg += ": " + result.Message;
                    }

                    alert(errorMsg);
                    vm.buildOrder = null;
		            vm.isOwner = false;
		        });
		}

        function _loadBuildItems(buildId) {
            buildOrderService.getBuildItemsForBuild(buildId)
                .success(function (results) {
                    if (results != null && results.length > 0) {
                        vm.loadedBuildItems = results;

                        _setFilteredBuildItems();

                        vm.buildItems.forEach(function (element, index, array) {
                            element.LogoPath = _getLogoByName(element.Name);
                        });
                    }
                });
        }

        function _setFilteredBuildItems() {
            if (vm.showWorkers) {
                vm.buildItems = vm.loadedBuildItems;
            } else {
                var filteredResults = [];
                for (var i = 0; i < vm.loadedBuildItems.length; i++) {
                    if (vm.loadedBuildItems[i].Name.toLowerCase() != "scv"
                        && vm.loadedBuildItems[i].Name.toLowerCase() != "probe"
                        && vm.loadedBuildItems[i].Name.toLowerCase() != "drone") {
                        filteredResults.push(vm.loadedBuildItems[i]);
                    }
                }
                vm.buildItems = filteredResults;
            }
        }

        function showWorkersChanged() {
            _setFilteredBuildItems();
        }

        function _getLogoByName(name) {
            for (var i = 0; i < buildItemsImages.length; i++) {
                if (buildItemsImages[i].name == name) {
                    return buildItemsImages[i].src + ".png";
                }
            }

            return "empty_cell.png";
        }

		function getImageForMatchup(matchup) {
		    return "/images/factions/" + matchup + ".png";
		}

		function getAddonByVersion(version) {
		    var number = version.replace(/\./g, '');

            if (number <= 205) {
                return "Wings of Liberty";
            } else if (number > 205 && number <= 220) {
                return "Heart of the Swarm";
            } else if (number >= 300) {
                return "Legacy of the Void";
            }
        }

		function deleteBuildOrder(buildOrder) {
		    buildOrderService.deleteBuildOrder(buildOrder)
                .success(function (result) {
                    $location.path("/home");
                });
        }
	}
})();