(function() {
    'use strict';

    angular.module('Sc2bmApp').service('commentService', commentService);

    commentService.$inject = ['resource', 'commentBackendUrls'];

    function commentService(resource, urls) {
        return {
            getComments: getComments,
            addComment: addComment,
            getByID: getByID,
            updateComment: updateComment,
            deleteComment: deleteComment,
            getLatestComments: getLatestComments
        };

        //region Methods
        function getComments(entityType, entityID) {
            return resource.get(urls.getComments, { entityType: entityType, entityID: entityID }, null, false);
        }

        function getLatestComments(entityType) {
            return resource.get(urls.getLatestComments, { entityType: entityType }, null, false);
        }

        function addComment(comment) {
            return resource.post(urls.addComment, { comment: comment }, null, false);
        }

        function getByID(id) {
            return resource.get(urls.getByID, { id: id }, null, false);
        }

        function updateComment(comment) {
            return resource.post(urls.updateComment, { comment: comment }, null, false);
        }

        function deleteComment(comment) {
            return resource.post(urls.deleteComment, { comment: comment }, null, false);
        }
    	//endregion
    }
})();