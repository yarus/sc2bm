(function() {
    'use strict';

    angular.module('Sc2bmApp').constant('commentBackendUrls', {
        getComments: 'api/Comment/GetComments',
        addComment: 'api/Comment/AddComment',
        updateComment: 'api/Comment/UpdateComment',
        deleteComment: 'api/Comment/DeleteComment',
        getByID: 'api/Comment/GetByID',
        getLatestComments: 'api/Comment/GetLatestComments'
    });
})();