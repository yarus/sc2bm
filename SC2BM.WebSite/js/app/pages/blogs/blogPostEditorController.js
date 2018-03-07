(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('blogPostEditorController', blogPostEditorController);

	blogPostEditorController.$inject = ['$routeParams', '$location', 'session', 'blogService', 'blogPostService'];

	function blogPostEditorController($routeParams, $location, session, blogService, blogPostService) {
		//region ViewModel declaration
		/*jshint validthis:true */
	    var vm = this;
	    vm.ownerUserName = null;
	    vm.personalBlog = {};
	    vm.user = {};

	    vm.addOrUpdateBlogPost = addOrUpdateBlogPost;

		activate();
		//endregion

		//region Methods
		function activate() {
		    vm.user = session.getUser();

		    vm.toolbar = [
                ['h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'p', 'pre', 'quote'],
                ['bold', 'italics', 'underline', 'strikeThrough', 'ul', 'ol', 'redo', 'undo', 'clear'],
                ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull', 'indent', 'outdent'],
                ['html', 'insertImage', 'insertLink']
		    ];

		    vm.ownerUserName = $routeParams.owner;
		    vm.blogPostId = $routeParams.blogPostId;

		    if (vm.ownerUserName == null || vm.user == null || vm.user.ID == null || (!session.isCurrentUserAdmin() && vm.user.UserName != vm.ownerUserName)) {
		        $location.path("/home");
		        return;
		    }

		    _loadData();
		}

		function _loadData() {
		    blogService.getByOwner(vm.ownerUserName)
                .success(function (result) {
                    if (result != null) {
                        vm.personalBlog = result;
                        _loadBlogPost();
                    }
                });
        }

		function _loadBlogPost() {
		    if (vm.blogPostId == null) {
		        vm.blogPost = {
		            Title: "",
		            Text: "",
		            OwnerUserID: vm.user.ID,
		            BlogID: vm.personalBlog.ID,
		            LogoPath: ""
		        };

		        return;
		    }

		    blogPostService.getById(vm.blogPostId)
		        .success(function (result) {
		            if (result != null) {
		                vm.blogPost = result;
		            }
		        });
		}
        
		function addOrUpdateBlogPost() {
		    if (vm.blogPost.Title == null || vm.blogPost.Title == "") {
		        alert("Title was not provided!");
		        return;
		    }

		    if (vm.blogPost.Text == null || vm.blogPost.Text == "") {
		        alert("Text was not provided!");
		        return;
		    }

		    if (vm.blogPost.OwnerUserID == null) {
                alert("Anauthorized access denied!");
                return;
            }

		    blogPostService.addOrUpdate(vm.blogPost)
                .success(function (result) {
                    if (vm.blogPost.ID != null) {
                        $location.path("/blogs/" + vm.user.UserName + "/" + vm.blogPost.ID);
                    } else if (result != null) {
                        $location.path("/blogs/" + vm.user.UserName + "/" + result);
                    }
                })
                .error(function (result) {
                    alert("There was an issue while updating blog. Please contact administrator.");
                });
		}
	}
})();