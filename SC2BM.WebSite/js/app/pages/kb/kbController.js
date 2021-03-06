﻿(function () {
    'use strict';

    angular.module('Sc2bmApp').controller('kbController', kbController);

    kbController.$inject = ['$routeParams', '$location', 'buildOrderService', 'buildItemsImages'];

    function kbController($routeParams, $location, buildOrderService, buildItemsImages) {
        //region ViewModel declaration
        /*jshint validthis:true */
        var vm = this;
        vm.version = "LOTV";
        vm.faction = "Terran";
        vm.itemType = "Units";
        vm.info = null;
        vm.visibleItems = null;
        vm.setFaction = setFaction;
        vm.setVersion = setVersion;
        vm.setItemType = setItemType;

        activate();
        //endregion

        //region Methods
        function activate() {
            if ($routeParams.version != null && $routeParams.version != "" && ['lotv', 'hots', 'wol'].indexOf($routeParams.version.toLowerCase()) != -1) {
                vm.version = $routeParams.version.toUpperCase();
            }

            if ($routeParams.faction != null && $routeParams.faction != "" && ['terran', 'protoss', 'zerg'].indexOf($routeParams.faction.toLowerCase()) != -1) {
                var formattedString = $routeParams.faction[0].toUpperCase() + $routeParams.faction.substr(1, $routeParams.faction.length - 1);
                vm.faction = formattedString;
            }

            if ($routeParams.itemType != null && $routeParams.itemType != "" && ['units', 'buildings', 'upgrades'].indexOf($routeParams.itemType.toLowerCase()) != -1) {
                vm.itemType = $routeParams.itemType;
            }

            _loadKbData();
        }

        function setFaction(faction) {
            vm.faction = faction;

            var url = "/kb/" + vm.version + "/" + faction;

            $location.path(url);

            // _loadKbData();
        }

        function setItemType(itemType) {
            vm.itemType = itemType;

            var url = "/kb/" + vm.version + "/" + vm.faction + "/" + vm.itemType;

            $location.path(url);

            // _setVisibleItems();
        }

        function _setVisibleItems() {
            if (vm.itemType.toLowerCase() == "units") {
                vm.visibleItems = vm.info.Units;
            } else if (vm.itemType.toLowerCase() == "buildings") {
                vm.visibleItems = vm.info.Buildings;
            } else if (vm.itemType.toLowerCase() == "upgrades") {
                vm.visibleItems = vm.info.Upgrades;
            }
        }

        function setVersion(version) {
            vm.version = version;

            var url = "/kb/" + vm.version;

            $location.path(url);

            //_loadKbData();
        }

        function _loadKbData() {
            var versionId = "5.0.3";

            if (vm.version == "HOTS") {
                versionId = "2.2.0";
            } else if (vm.version == "WOL") {
                versionId = "2.0.5";
            }

            buildOrderService.getKbData(versionId, vm.faction)
                .success(function (result) {
                    vm.info = result;

                    vm.info.Units.forEach(setLogoImage);
                    vm.info.Buildings.forEach(setLogoImage);
                    vm.info.Upgrades.forEach(setLogoImage);

                    _setVisibleItems();
                });
        }

        function setLogoImage(element, index, array) {
            element.LogoPath = _getLogoByName(element.Name);

            if (element.Requirements.length > 0) {
                element.Requirements.forEach(function (el, index, array) {
                    el.LogoPath = _getLogoByName(el.Name);
                });
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
    }
})();