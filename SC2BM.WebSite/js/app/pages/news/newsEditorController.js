(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('newsEditorController', newsEditorController);

	newsEditorController.$inject = ['$routeParams', '$location', 'session', 'newsService'];

	function newsEditorController($routeParams, $location, session, newsService) {
		//region ViewModel declaration
		/*jshint validthis:true */
	    var vm = this;
	    vm.newsItem = {Title: "", Text: "", OwnerUserID: null, AddedDate: '0'};
	    vm.user = {};
	    vm.addNews = addNews;
	    vm.setLogo = setLogo;
	    vm.textChanged = textChanged;

		activate();
		//endregion

		//region Methods
		function activate() {
		    vm.user = session.getUser();

		    vm.toolbar = [
		        ['h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'p', 'pre', 'quote'],
		        ['bold', 'italics', 'underline', 'strikeThrough', 'ul', 'ol', 'redo', 'undo', 'clear'],
		        ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull', 'indent', 'outdent'],
		        ['html', 'insertImage', 'insertLink', 'insertVideo']
		    ];

		    vm.newsID = $routeParams.id;

		    if (vm.newsID == null && (vm.user == null || vm.user.ID == null || vm.user.Roles.indexOf("Admin") == -1)) {
		        $location.path("/home");
		        return;
		    } else if (vm.newsID != null) {
		        vm.newsRequest = { pageNumber: 1, rowsPerPage: 1, totalItems: 0, orderBy: "AddedDate", orderByAscending: false, filter: { NewsItemID: vm.newsID } };

		        _loadNews();
		    } else {
		        vm.newsItem.OwnerUserID = vm.user.ID;
		        vm.newsItem.LogoPath = "/images/news/default/update.png";
		    }
		}

		function textChanged(text) {
		    vm.text = text.replace(/\n/g, '<br/>');
		}

		function _loadNews() {
		    newsService.searchNews(vm.newsRequest)
                .success(function (result) {
                    if (result.Items.length > 0) {
                        vm.newsItem = result.Items[0];
                        vm.isOwner = vm.newsItem.OwnerUserID == vm.user.ID || session.isCurrentUserAdmin();

                        if (vm.newsItem.Text != null) {
                            vm.text = vm.newsItem.Text.replace(/\n/g, '<br/>');
                        }

                        if (!vm.isOwner) {
                            $location.path("/home");
                            return;
                        }
                    }
                    vm.newsRequest.totalItems = result.TotalCount;
                })
		        .error(function(result) {
		            $location.path("/home");
		        });
		}

		function addNews() {
		    if (vm.newsItem.Title == null || vm.newsItem.Title == "") {
		        alert("Title was not provided!");
		        return;
		    }

		    if (vm.newsItem.Text == null || vm.newsItem.Text == "") {
		        alert("Text was not provided!");
		        return;
		    }

            if (vm.newsItem.OwnerUserID == null) {
                alert("Anauthorized access denied!");
                return;
            }

		    newsService.addOrUpdateNews(vm.newsItem)
                .success(function (result) {
                    $location.path("/home");
                })
                .error(function (result) {
                    alert("There was an issue while posting news. Please contact administrator.");
                });
		}

		function setLogo(name) {
		    var path = "/images/news/default/";

            if (name.toLowerCase() == "patch") {
                path += "sc2patch.png";
            } else if (name.toLowerCase() == "update") {
                path += "update.png";
            } else if (name.toLowerCase() == "config") {
                path += "config.png";
            } else if (name.toLowerCase() == "vod") {
                path += "vod.png";
            } else if (name.toLowerCase() == "info") {
                path += "info.png";
            }

		    vm.newsItem.LogoPath = path;
		}
	}
})();