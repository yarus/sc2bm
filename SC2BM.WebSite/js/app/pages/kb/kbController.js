(function () {
    'use strict';

    angular.module('Sc2bmApp').controller('kbController', kbController);

    kbController.$inject = ['$routeParams', 'buildOrderService', 'buildItemsImages'];

    function kbController($routeParams, buildOrderService, buildItemsImages) {
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

            _loadKbData();
        }

        function setItemType(itemType) {
            vm.itemType = itemType;

            _setVisibleItems();
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

            _loadKbData();
        }

        function _loadKbData() {
            var versionId = "4.11.3";

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