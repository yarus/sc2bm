(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('videoListController', videoListController);

	videoListController.$inject = ['$routeParams', '$location', 'session', 'linkService', 'twitchService'];

	function videoListController($routeParams, $location, session, linkService, twitchService) {
		//region ViewModel declaration
		/*jshint validthis:true */
	    var vm = this;
	    vm.user = {};
	    vm.rights = {
	        enableAddVideo: false
	    };

	    vm.vods = [];

	    vm.deleteLink = deleteLink;
	    vm.loadMore = loadMore;

	    var rowsPerPageMultiplier = 4;

	    activate();
		//endregion

		//region Methods
		function activate() {
		    vm.user = session.getUser();

		    vm.rights.enableAddVideo = session.isUserSignedIn();

		    vm.linksRequest = {
		        pageNumber: 1, rowsPerPage: 12, totalItems: 0, orderBy: "AddedDate", orderByAscending: false,
		        filter: { Type: "vod" }
		    };

		    _loadVods();
		}

		function loadMore() {
		    vm.linksRequest.rowsPerPage += rowsPerPageMultiplier;
		    _loadVods();
		}

		function deleteLink(link) {
		    if (link == null || link.ID == null) {
		        return;
		    }

		    linkService.deleteLink(link)
		        .success(function (result) {
		            _loadVods();
		        });
		}

        function _loadVods() {
            linkService.searchLinks(vm.linksRequest)
                .success(function (result) {
                    if (result != null && result.Items != null) {
                        vm.vods = result.Items;
                        vm.linksRequest.totalItems = result.TotalCount;

                        vm.vods.forEach(function (element, index, array) {
                            _setThumbUrl(element);
                            _setEntityLink(element);
                            _setDeleteEnabled(element);
                        });
                    }
                });
        }

        function _setDeleteEnabled(element) {
            element.deleteEnabled = (session.isCurrentUserAdmin() || (vm.user != null && element.OwnerUserID == vm.user.ID));
        }

        function _setThumbUrl(element) {
            if (element.MainLink.contains("youtu")) {
                var items = element.MainLink.split('/');
                var splitVoidIdWithParams = items[items.length - 1].split('?');
                var vodYouTubeId = splitVoidIdWithParams[0];
                element.thumbUrl = "http://img.youtube.com/vi/" + vodYouTubeId + "/mqdefault.jpg";
            } else if (element.MainLink.contains("twitch")) {
                var el = element;
                twitchService.getVideo(element.MainLink)
                    .success(function (result) {
                        if (result != null && result.preview != null) {
                            el.thumbUrl = result.preview;
                        }
                    });
            }
        }

        function _setEntityLink(element) {
            if (element.EntityType == "Build") {
                element.EntityLink = "/builds/" + element.EntityID;
                element.EntityLinkText = "Open Linked Build";
            }
        }
	}
})();