(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('blogEditorController', blogEditorController);

	blogEditorController.$inject = ['$routeParams', '$location', 'session', 'blogService'];

	function blogEditorController($routeParams, $location, session, blogService) {
		//region ViewModel declaration
		/*jshint validthis:true */
	    var vm = this;
	    vm.user = {};
	    vm.settings = {
	        description: {
	            maxLength: 1000,
	            maxLengthError: "maxhtmllength",
	            toolbar: [
                    ['h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'p', 'pre', 'quote'],
                    ['bold', 'italics', 'underline', 'strikeThrough', 'ul', 'ol', 'redo', 'undo', 'clear'],
                    ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull', 'indent', 'outdent'],
                    ['html', 'insertImage', 'insertLink']
	            ]
	        }
	    };

	    vm.ownerUserName = null;
	    vm.personalBlog = {};

	    vm.addOrUpdateBlog = addOrUpdateBlog;
	    vm.descriptionChanged = descriptionChanged;

		activate();
		//endregion

		//region Methods
		function activate() {
		    vm.user = session.getUser();

		    vm.ownerUserName = $routeParams.owner;

		    if (vm.ownerUserName == null || vm.user == null || vm.user.ID == null || vm.user.UserName != vm.ownerUserName) {
		        $location.path("/home");
		        return;
		    } else {
		        _loadPersonalBlog();
		    }
		}

		function _loadPersonalBlog() {
		    if (vm.user == null || vm.user.ID == null) {
		        return;
		    }

		    blogService.getByOwner(vm.ownerUserName)
		        .success(function (result) {
		            if (result != null) {
		                vm.personalBlog = result;
		            } else {
		                vm.personalBlog = {
		                    Title: "",
		                    Description: "",
		                    OwnerUserID: vm.user.ID,
		                    LogoPath: ""
		                }
		            }
		        });
		}
        
		function descriptionChanged(newValue) {
		    if (newValue.length > vm.settings.description.maxLength) {
		        vm.mainForm.description.$setValidity(vm.settings.description.maxLengthError, false);
		    } else {
		        vm.mainForm.description.$setValidity(vm.settings.description.maxLengthError, true);
		    }
		}

		function addOrUpdateBlog() {
		    if (vm.personalBlog.Title == null || vm.personalBlog.Title == "") {
		        alert("Title was not provided!");
		        return;
		    }

		    if (vm.personalBlog.Description == null || vm.personalBlog.Description == "") {
		        alert("Description was not provided!");
		        return;
		    }

		    if (vm.personalBlog.OwnerUserID == null) {
                alert("Anauthorized access denied!");
                return;
            }

		    blogService.addOrUpdate(vm.personalBlog)
                .success(function (result) {
                    $location.path("/blogs/" + vm.user.UserName);
                })
                .error(function (result) {
                    alert("There was an issue while updating blog. Please contact administrator.");
                });
		}
	}
})();