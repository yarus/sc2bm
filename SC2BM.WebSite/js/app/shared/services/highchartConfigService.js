(function() {
    'use strict';

    angular.module('Sc2bmApp').service('highchartConfigService', highchartConfigService);

    function highchartConfigService() {
        //region Public Api
        var publicApi = {
            getLineChartConfig: getLineChartConfig
        };

        return publicApi;
        //endregion

        function getLineChartConfig(series) {
            return _getConfig(series, 'line');
        }

        function _getConfig(series, chartType, title, width, height) {
            return {
                options: {
                    chart: {
                        type: chartType
                    },
                    plotOptions: {
                        series: {
                            stacking: ''
                        }
                    }
                },
                series: series,
                title: {
                    text: title || ''
                },
                credits: {
                    enabled: false
                },
                loading: false,
                size: {
                    width:width || 300,
                    height:height || 200
                }
            };
        }
    }
})();