(function () {
    'use strict';
    angular.module('Sc2bmApp').filter('htmlToTextWithLines', htmlToTextWithLines);

    htmlToTextWithLines.$inject = ['$filter'];

    function htmlToTextWithLines($filter) {
        return function (html) {
            var tmp = html.replace(/<br *\/?>/gi, '\n');
            tmp = tmp.replace(/<\/p>/gi, '\n');
            tmp = tmp.replace(/<\/h1>/gi, '\n');
            tmp = tmp.replace(/<\/h2>/gi, '\n');
            tmp = tmp.replace(/<\/h3>/gi, '\n');
            tmp = tmp.replace(/<\/h4>/gi, '\n');
            tmp = tmp.replace(/<\/h5>/gi, '\n');
            tmp = tmp.replace(/<\/h6>/gi, '\n');
            tmp = $filter('htmlToPlainText')(tmp);

            return tmp;
        }
    }
})();