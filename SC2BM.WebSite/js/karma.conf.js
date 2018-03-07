// Karma configuration
// Generated on Fri Nov 28 2014 16:07:02 GMT+0300 (Russia TZ 2 Standard Time)

module.exports = function(config) {
  'use strict';

  config.set({

    // base path that will be used to resolve all patterns (eg. files, exclude)
    basePath: '../',

    // frameworks to use
    // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
    frameworks: ['jasmine'],

    // list of files / patterns to load in the browser
    files: [
      'js/lib/jquery-2.1.1.js',
      'js/lib/underscore.js',
      'js/lib/underscore.string.js',
      'js/lib/jquery-ui.js',
      'js/lib/jquery.ui.touch-punch.js',
      'js/lib/angular.js',
      'js/lib/angular-sortable.js',
      'js/lib/angular-resource.js',
      'js/lib/angular-route.js',
      'js/lib/angular-mocks.js',
      'js/lib/angular-filter.js',
      'js/lib/highcharts.js',
      'js/lib/highcharts-more.js',
      'js/lib/solid-gauge.js',
      'js/lib/angular-highcharts.js',
      'js/lib/angular-sanitize.js',
      'js/lib/phoneFormat.js',
      'js/lib/jasmine-utils.js',
      'js/app/shell/app.js',
      'js/app/**/*.js',
      'js/app/**/*.spec.js',
      'js/app/**/*.html'
    ],

    // list of files to exclude
    exclude: [
        'js/lib/jasmine.js',
        'karma.conf.js'
    ],

    // preprocess matching files before serving them to the browser
    // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
    preprocessors: {
      'js/app/**/*.html': 'ng-html2js',
      'js/app/**/*.js': 'coverage'
    },

    // test results reporter to use
    // possible values: 'dots', 'progress'
    // available reporters: https://npmjs.org/browse/keyword/karma-reporter
    reporters: ['progress'],

    // web server port
    port: 9876,


    // enable / disable colors in the output (reporters and logs)
    colors: true,


    // level of logging
    // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
    logLevel: config.LOG_INFO,


    // enable / disable watching file and executing tests whenever any file changes
    autoWatch: false,


    // start these browsers
    // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
    browsers: ['IE'],


    // Continuous Integration mode
    // if true, Karma captures browsers, runs the tests and exits
    singleRun: true,

    plugins: [
      'karma-ie-launcher',
      'karma-jasmine',
      'karma-ng-html2js-preprocessor',
      'karma-coverage'
    ]
  });
};
