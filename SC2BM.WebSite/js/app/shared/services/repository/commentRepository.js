(function () {
    'use strict';

    angular.module('Sc2bmApp').service('commentRepository', commentRepository);

    commentRepository.$inject = ['$q', 'commentService'];

    function commentRepository($q, commentService) {
        var totalComments = [];

        return {
            reset: reset,
            getTotalComments: getTotalComments
        };

        //region Methods
        function reset() {
            totalComments = [];
        }

        function getTotalComments(entityType, entityId) {
            var deferred = $q.defer();

            var item = _.find(totalComments, function (o) { return o.entityType == entityType && o.entityId == entityId; });
            if (item == null) {
                commentService.getComments(entityType, entityId)
                    .success(function (result) {
                        if (result != null) {
                            var el = {};
                            el.value = result.TotalCount;
                            el.entityType = entityType;
                            el.entityId = entityId;
                            totalComments.push(el);
                            deferred.resolve(el.value);
                        }
                    })
                    .error(function (result) {
                        deferred.resolve(null);
                    });
            } else {
                deferred.resolve(item.value);
            }

            return deferred.promise;
        }
        //endregion
    }
})();