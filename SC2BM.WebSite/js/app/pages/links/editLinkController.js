(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('editLinkController', editLinkController);

	editLinkController.$inject = ['$routeParams', '$location', '$filter', 'session', 'linkService'];

	function editLinkController($routeParams, $location, $filter, session, linkService) {
		//region ViewModel declaration
		/*jshint validthis:true */
	    var vm = this;
	    vm.user = {};
	    vm.link = null;
	    vm.saveLink = saveLink;
	    vm.isEdit = false;

	    vm.types = ["vod", "replay", "href"];

		activate();
		//endregion

		//region Methods
		function activate() {
		    vm.user = session.getUser();

		    if (vm.user == null || vm.user.ID == null) {
		        $location.path("/home");
		        return;
		    }

		    vm.linkID = $routeParams.id;
		    vm.entityType = $routeParams.entityType;
		    vm.entityId = $routeParams.entityId;
		    vm.linkType = $routeParams.linkType;

		    if (vm.linkID == null) {
		        vm.isEdit = false;
		        _bindLink({ EntityType: vm.entityType, EntityID: vm.entityId, OwnerUserID: vm.user.ID, DisplayText: "", Type: vm.linkType, MainLink: "", SecondaryLink: "", AddedDate: new Date() });
		    } else {
		        vm.isEdit = true;
		        _loadLink(vm.linkID);
		    }
		}

        function _bindLink(link) {
            vm.link = link;
            vm.createdDate = $filter('date')(vm.link.AddedDate, 'fullDate');
        }

        function _loadLink(linkID) {

            linkService.getByID(linkID)
                .success(function (result) {
                    if (result != null && (session.isCurrentUserAdmin() || result.OwnerUserID == vm.user.ID)) {
                        _bindLink(result);
                    } else {
                        $location.path("/home");
                        return;
                    }
                })
                .error(function (result) {
                    var errorMsg = "Error while loading link.";

                    if (result != null) {
                        errorMsg += ": " + result.Message;
                    }

                    alert(errorMsg);
                    vm.link = null;
                });
        }

        function _navigateToEntity(entityType, entityID) {
            if (entityType == "Build") {
                $location.path("/builds/" + entityID);
            } else {
                $location.path("/home");
            }
        }

        function saveLink(link) {
            var entityType = link.EntityType;
            var entityID = link.EntityID;

            if (vm.isEdit) {
                linkService.updateLink(link)
                    .success(function (result) {
                        _navigateToEntity(entityType, entityID);
                    })
                    .error(function (result) {
                        var errorMsg = "Error while saving link";

                        if (result != null) {
                            errorMsg += ": " + result.Message;
                        }

                        alert(errorMsg);
                    });
            } else {
                linkService.addLink(link)
                     .success(function (result) {
                         _navigateToEntity(entityType, entityID);
                     })
                     .error(function (result) {
                         var errorMsg = "Error while adding link";

                         if (result != null) {
                             errorMsg += ": " + result.Message;
                         }

                         alert(errorMsg);
                     });
            }
        }
	}
})();