(function () {
	'use strict';

	angular.module('Sc2bmApp').controller('commentsController', commentsController);

	commentsController.$inject = ['$scope', '$attrs', '$parse', 'commentService'];

	function commentsController($scope, $attrs, $parse, commentService) {
	    //region ViewModel declaration
	    /*jshint validthis:true */
	    var vm = this;
	    vm.user = $scope.user;
	    vm.entity = $scope.entity;
	    vm.type = $scope.type;
	    vm.outputModel = $scope.count;

	    vm.editComment = null;
	    vm.comments = [];

	    vm.reply = reply;
	    vm.postComment = postComment;
	    vm.startEditComment = startEditComment;
	    vm.saveEdit = saveEdit;
	    vm.cancelEdit = cancelEdit;
	    vm.deleteComment = deleteComment;

	    activate();

	    function activate() {
	        vm.isAdmin = (vm.user != null && vm.user.ID != null && vm.user.Roles != null && vm.user.Roles.length > 0 && vm.user.Roles.indexOf("Admin") != -1);

	        //This also happens to be the default menu options.
	        vm.toolbar = [
		        ['p', 'pre', 'quote'],
		        ['bold', 'italics', 'underline', 'strikeThrough', 'ul', 'ol'],
		        ['insertImage', 'insertLink'],
                ['redo', 'undo', 'clear']
	        ];

	        $scope.$watch(
                function() {
                    return ($scope.entity);
                },
                function (newValue, oldValue) {
                    vm.entity = newValue;

	                _loadComments();
                });

	        $scope.$watch(
                function () {
                    return ($scope.user);
                },
                function (newValue, oldValue) {
                    vm.user = newValue;
                    vm.isAdmin = (vm.user != null && vm.user.ID != null && vm.user.Roles != null && vm.user.Roles.length > 0 && vm.user.Roles.indexOf("Admin") != -1);
                });
	    }

        function _loadComments() {
            if (vm.entity == null || vm.entity.ID == null) {
                vm.outputModel.commentsTotal = 0;
                return;
            }

	        commentService.getComments(vm.type, vm.entity.ID)
                .success(function (result) {
                    vm.comments = result.Items;
                    vm.outputModel.commentsTotal = result.TotalCount;

                    vm.comments.forEach(function (element, index, array) {
                        if (element.Text != null) {
                            //element.Text = element.Text.replace(/\n/g, '<br/>');
                        }
                    });
                });
	    }

	    function saveEdit(editComment, newText) {
	        if (editComment == null || newText == "") {
	            alert("Text is required!");
	            return;
	        }

	        editComment.Text = newText;

	        commentService.updateComment(editComment)
		        .success(function (result) {
		            vm.comment = "";
		            _loadComments();
		        });

	        vm.editComment = null;
	    }

	    function cancelEdit() {
	        vm.editComment = null;
	        vm.comment = "";
	    }

	    function startEditComment(comment) {
	        vm.editComment = comment;
	        vm.comment = comment.Text;
	    }

	    function deleteComment(comment) {
	        if (comment == null) {
	            return;
	        }

	        commentService.deleteComment(comment)
                .success(function (result) {
                    _loadComments();
                });

	        vm.comment = "";
	    }

	    function reply(comment) {
	        vm.comment = "<blockquote><p>" + comment.Text + "</p></blockquote><br />";
	    }

	    function postComment(comment) {
	        if (comment == null || comment == "") {
	            return;
	        }

	        var item = { Text: comment, OwnerUserID: vm.user.ID, EntityType: vm.type, EntityID: vm.entity.ID };

	        commentService.addComment(item)
		        .success(function (result) {
		            vm.comment = "";
		            _loadComments();
		        });
	    }
	}
})();
