(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('blogViewController', blogViewController);

	blogViewController.$inject = ['$routeParams', '$location', '$filter', 'session', 'blogService', 'blogPostService', 'commentService'];

	function blogViewController($routeParams, $location, $filter, session, blogService, blogPostService, commentService) {
		//region ViewModel declaration
		/*jshint validthis:true */
	    var vm = this;
	    vm.user = {};
	    vm.isOwner = false;

	    vm.blog = {};
	    vm.blogPostsRequest = {};
	    vm.blogPosts = [];
	    vm.topRatedPosts = [];
	    vm.latestComments = [];
	    vm.latestLinks = [];

	    vm.showMorePosts = showMorePosts;

	    var postsCountStep = 5;
	    var postsCount = postsCountStep;

		activate();
		//endregion

		//region Methods
		function activate() {
		    vm.user = session.getUser();

		    vm.blogOwner = $routeParams.owner;

            if (vm.blogOwner == null) {
                $location.path("/home");
                return;
            }

            vm.enableNewPost = (vm.user != null && vm.user.UserName != null && vm.blogOwner == vm.user.UserName);
		    vm.enableEditBlog = vm.enableNewPost;

		    _loadBlog();
		}

		function showMorePosts() {
		    postsCount = postsCount + postsCountStep;

		    _loadBlogPosts();
		}

		function _loadLatestComments() {
		    blogService.getBlogComments(vm.blog.ID)
                .success(function (result) {
                    if (result != null) {
                        vm.latestComments = result;

                        vm.latestComments.forEach(function (element, index, array) {
                            if (element.Text != null) {
                                element.Text = $filter('htmlToPlainText')(element.Text);

                                if (element.Text.length > 100) {
                                    element.Text = element.Text.substring(0, 100);
                                }
                            }

                            if (element.EntityType == "BlogPost") {
                                element.EntityLink = "/blogs/" + vm.blogOwner + "/" + element.EntityID;
                            }
                        });
                    }
                });
		}

        function _loadBlog() {
            blogService.getByOwner(vm.blogOwner)
                .success(function(result) {
                    if (result != null) {
                        vm.blog = result;

                        vm.blog.Description = vm.blog.Description.replace(/\n/g, '<br/>');

                        blogService.getRateForBlog(vm.blog.ID)
                            .success(function (result) {
                                if (result != null) {
                                    vm.blog.Rate = result.Value;
                                }
                            });

                        _loadBlogPosts();
                        _loadLatestComments();
                        _loadTopRatedPosts();
                    }
                });
        }

        function _loadTopRatedPosts() {
            blogPostService.getTopRatedBlogPosts(vm.blog.ID, 5)
                .success(function (result) {
                    if (result != null && result.length > 0) {
                        vm.topRatedPosts = result;

                        vm.topRatedPosts.forEach(function (element, index, array) {
                            var el = element;
                            blogPostService.getRateForBlogPost(element.ID)
                                .success(function(result) {
                                    if (result != null && result.Value != '') {
                                        el.rate = parseFloat(result.Value).toFixed(1);
                                    }
                                });

                            if (element.Text != null) {
                                element.Text = $filter('htmlToPlainText')(element.Text);

                                if (element.Text.length > 40) {
                                    element.Text = element.Text.substring(0, 40) + "...";
                                }
                            }
                        });
                    }
                });
        }

        function _loadBlogPosts() {
            vm.blogPostsRequest = {
                pageNumber: 1, rowsPerPage: postsCount, orderBy: "AddedDate", orderByAscending: false,
                filter: {
                    blogID: vm.blog.ID,
                    IsDeleted: false
                }
            };

            blogPostService.search(vm.blogPostsRequest)
                .success(function(result) {
                    if (result != null && result.Items != null) {
                        vm.blogPosts = result.Items;
                        vm.blogPostsTotalCount = result.TotalCount;
                        vm.blogPosts.forEach(function (element, index, array) {
                            element.logoVisible = (element.LogoPath != null);

                            var el = element;
                            commentService.getComments('BlogPost', element.ID)
                                .success(function (result) {
                                    if (result != null) {
                                        el.commentsTotal = result.TotalCount;
                                    }
                                });
                        });
                    }
                });
        }
	}
})();