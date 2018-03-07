(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('blogPostViewController', blogPostViewController);

	blogPostViewController.$inject = ['$routeParams', '$location', '$filter', 'session', 'blogPostService', 'commentService', 'rateService', 'linkService'];

	function blogPostViewController($routeParams, $location, $filter, session, blogPostService, commentService, rateService, linkService) {
		//region ViewModel declaration
		/*jshint validthis:true */
	    var vm = this;
	    vm.user = {};
	    vm.isOwner = false;
	    vm.commentsModel = {};

	    vm.ownerUserName = null;
	    vm.blogPostId = null;

	    vm.blogPost = {};

	    vm.text = "";

	    vm.deleteRate = deleteRate;
	    vm.addRate = addRate;
	    vm.deleteLink = deleteLink;
	    vm.deleteBlogPost = deleteBlogPost;
	    vm.rate = "0.0";
	    vm.logoVisible = false;

		activate();
		//endregion

		//region Methods
		function activate() {
		    vm.user = session.getUser();

		    vm.ownerUserName = $routeParams.owner;
		    vm.blogPostId = $routeParams.blogPostId;

		    if (vm.ownerUserName == null || vm.blogPostId == null) {
                $location.path("/home");
                return;
		    }

		    _loadBlogPost();
		    _loadTotalRate();
		    _loadPersonalRate();
		    _loadLinks();
		}

        function _loadBlogPost() {
            blogPostService.getById(vm.blogPostId)
                .success(function(result) {
                    if (result != null) {
                        vm.blogPost = result;
                        vm.createdDate = $filter('date')(vm.blogPost.AddedDate, 'fullDate');
                        vm.isOwner = (vm.user != null && vm.blogPost.OwnerUserID == vm.user.ID) || session.isCurrentUserAdmin();
                        vm.logoVisible = (vm.blogPost.LogoPath != null);
                    }
                });
        }

        function _loadTotalRate() {
            rateService.getTotalRate('BlogPost', vm.blogPostId)
                .success(function (result) {
                    if (result != null && result != '') {
                        vm.rate = parseFloat(result).toFixed(1);
                    } else {
                        vm.rate = "0.0";
                    }
                });
        }

        function addRate(value) {
            var rate = { OwnerUserID: vm.user.ID, EntityID: vm.blogPostId, EntityType: 'BlogPost', Value: value };

            if (vm.personalRate == null) {
                rateService.addRate(rate)
                    .success(function () {
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

	    function _loadLinks() {
	        linkService.getLinks('BlogPost', vm.blogPostId)
	            .success(function(result) {
	                vm.vods = [];
	                vm.replays = [];
	                vm.hrefs = [];

	                result.Items.forEach(function(element, index, array) {
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

	    function deleteLink(link) {
	        if (link == null || link.ID == null) {
	            return;
	        }

	        linkService.deleteLink(link)
		        .success(function (result) {
		            _loadLinks();
		        });
	    }

	    function _loadPersonalRate() {
            if (vm.user == null || vm.user.ID == null) {
                return;
            }

            rateService.getPersonalRate('BlogPost', vm.blogPostId, vm.user.ID)
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
		        .success(function (result) {
		            vm.personalRate = null;
		            _loadTotalRate();
		        });
        }

        function deleteBlogPost(blogPost) {
            blogPostService.deleteBlogPost(blogPost)
                .success(function (result) {
                    $location.path("/home");
                });
        }
	}
})();