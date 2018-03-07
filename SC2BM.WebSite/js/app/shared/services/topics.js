(function () {
    'use strict';

    /**
     * @typedef {Object} topics
     * @property {string} noteRemoved
     */
    angular.module('Sc2bmApp').constant('topics', {
        noteRestored: 'NOTE_RESTORED',
        reportRequested: 'REPORT_REQUESTED'
    });
})();