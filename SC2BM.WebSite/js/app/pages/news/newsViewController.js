(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('newsViewController', newsViewController);

	newsViewController.$inject = ['$routeParams', '$location', 'session', 'newsService', 'commentService'];

	function newsViewController($routeParams, $location, session, newsService) {
		//region ViewModel declaration
		/*jshint validthis:true */
	    var vm = this;
	    vm.newsItem = {};
	    vm.user = {};
	    vm.isOwner = false;
	    vm.deleteNewsItem = deleteNewsItem;
	    vm.commentsModel = {};

		activate();
		//endregion

		//region Methods
		function activate() {
		    vm.user = session.getUser();

		    vm.newsID = $routeParams.id;

            if (vm.newsID == null) {
                $location.path("/home");
                return;
            }

            vm.newsRequest = { pageNumber: 1, rowsPerPage: 1, totalItems: 0, orderBy: "AddedDate", orderByAscending: false, filter: { NewsItemID: vm.newsID } };
		    
            _loadNews();
		}

        function _loadNews() {
            newsService.searchNews(vm.newsRequest)
                .success(function (result) {
                    if (result.Items.length > 0) {
                        vm.newsItem = result.Items[0];
                        if (vm.newsItem.Text != null) {
                            vm.text = vm.newsItem.Text.replace(/\n/g, '<br/>');
                        }
                        vm.isOwner = vm.newsItem.OwnerUserID == vm.user.ID || session.isCurrentUserAdmin();
                    }
                    vm.newsRequest.totalItems = result.TotalCount;
                });
        }

        function deleteNewsItem(newsItem) {
            newsService.deleteNews(newsItem)
                .success(function (result) {
                    $location.path("/home");
                });
        }
	}
})();