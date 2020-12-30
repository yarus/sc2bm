(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('editBuildController', editBuildController);

	editBuildController.$inject = ['$routeParams', '$location', '$filter', 'session', 'buildOrderService', 'Upload'];

	function editBuildController($routeParams, $location, $filter, session, buildOrderService, Upload) {
		//region ViewModel declaration
		/*jshint validthis:true */
	    var vm = this;
	    vm.user = {};
	    vm.buildOrder = null;
	    vm.hasErrors = false;
	    vm.saveBuildOrder = saveBuildOrder;

	    vm.matchup = "";
	    vm.matchupLogo = "";
	    vm.matchups = [];

	    vm.marks = [0, 1, 2, 3, 4, 5];

	    vm.isUpload = true;
	    vm.uploadFiles = uploadFiles;

		activate();
		//endregion

		//region Methods
		function activate() {
            vm.user = session.getUser();

            vm.isAdmin = session.isCurrentUserAdmin();

		    vm.toolbar = [
                ['h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'p', 'pre', 'quote'],
                ['bold', 'italics', 'underline', 'strikeThrough', 'ul', 'ol', 'redo', 'undo', 'clear'],
                ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull', 'indent', 'outdent'],
                ['html', 'insertImage', 'insertLink']
		    ];

		    if (vm.user == null || vm.user.ID == null) {
		        $location.path("/home");
		        return;
		    }

		    vm.buildID = $routeParams.id;

		    if (vm.buildID == null) {
		        vm.isUpload = true;
		    } else {
		        vm.isUpload = false;
		        _loadBuild(vm.buildID);
		    }
		}

        function _bindBuild(build) {
            vm.buildOrder = build;
            vm.buildItems = vm.buildOrder.BuildItems.join();
            vm.createdDate = $filter('date')(vm.buildOrder.AddedDate, 'fullDate');

            vm.isOwner = (vm.user != null && vm.buildOrder.OwnerUserID == vm.user.ID) || session.isCurrentUserAdmin();

            var raceLetter = vm.buildOrder.Race[0];

            vm.matchups = [];
            vm.matchups.push(raceLetter + "vT");
            vm.matchups.push(raceLetter + "vP");
            vm.matchups.push(raceLetter + "vZ");

            vm.matchup = vm.buildOrder.Race[0] + "v" + vm.buildOrder.VsRace[0];
        }

        function _loadBuild(buildID) {
            buildOrderService.getBuildByID(buildID)
                .success(function (result) {
                    if (result != null) {
                        _bindBuild(result);
                    } else {
                        alert("Build was not parsed correctly!");
                    }
                })
                .error(function (result) {
                    var errorMsg = "Error while loading build.";

                    if (result != null) {
                        errorMsg += ": " + result.Message;
                    }

                    alert(errorMsg);
                    vm.buildOrder = null;
                });
        }

        function _getFactionByLetter(letter) {
            if (letter == "T") {
                return "Terran";
            } else if (letter == "P") {
                return "Protoss";
            } else if (letter == "Z") {
                return "Zerg";
            }

            return "";
        }

        function saveBuildOrder(build) {
            var buildName = build.Name;
            var buildID = build.ID;
            build.VsRace = _getFactionByLetter(vm.matchup[2]);
            build.BuildItems = vm.buildItems;

            if (vm.isUpload) {
                buildOrderService.addBuildOrder(build)
                     .success(function (result) {
                         vm.uploadMessages.push(buildName + ' uploaded successfully');
                         vm.hasErrors = false;
                         vm.buildOrder = null;
                     })
                     .error(function (result) {
                         var errorMsg = "Error while uploading " + buildName;

                         if (result != null) {
                             errorMsg += ": " + result.Message;
                         }

                         vm.uploadMessages.push(errorMsg);
                         vm.hasErrors = true;
                         vm.buildOrder = null;
                     });
            } else {
                buildOrderService.updateBuildOrder(build)
                    .success(function (result) {
                        $location.path("/builds/" + buildID);
                    })
                    .error(function (result) {
                        var errorMsg = "Error while saving " + buildName;

                        if (result != null) {
                            errorMsg += ": " + result.Message;
                        }

                        alert(errorMsg);
                    });
            }
        }

        function uploadFiles(files, errFiles) {
            vm.uploadMessages = [];
            vm.hasErrors = false;

            angular.forEach(files, function (file) {
                file.upload = Upload.upload({
                    url: '/api/BuildOrder/GetUploadBuildDetails',
                    method: 'POST',
                    file: file
                }).success(function (data, status, headers, config) {
                    _bindBuild(data);
                    vm.hasErrors = false;
                }).error(function (err) {
                    var errorMsg = "Error while uploading " + file.name;

                    if (err != null) {
                        errorMsg += ": " + err.Message;
                    }

                    vm.uploadMessages.push(errorMsg);
                    vm.hasErrors = true;
                    vm.buildOrder = null;
                });
            });
        }
	}
})();