///////////////////////////////////////////////////////////////////////////////////
//Controller

function indexCtrl($scope,$interval, serv) {
    $scope.model = new Object();

    $scope.model.Respuesta = "Backend EDGAR";
    
    var stop;
    $scope.startMonitorinDatasets_click = function () {
        $scope.model.message = "mostrar datasets";
        /*
        serv.getDatasets(
            function (datasets) {
                $scope.model.datasets = datasets;
            }
        );
        */


        // Don't start a new fight if we are already fighting
        if (angular.isDefined(stop))
            return;
        $scope.model.monitoringDatasets = true;
        stop = $interval(function () {
            serv.getDatasets(
                function (rawDatasets) {
                    $scope.model.datasets = rawDatasets;
                    /*
                    var datasets = new Array(rawDatasets.length);
                    for (var i = 0; i < rawDatasets.length;i++)
                    {
                        datasets = getPercentageDataset(rawDatasets[i]);
                    }
                    $scope.model.datasets = datasets;
                    */
                    $scope.model.datasetsMessage = "Executing at: " + (new Date());
                }
            );
            
        }, 1000);
    }

    $scope.stopMonitorinDatasets_click = function () {
        if (angular.isDefined(stop)) {
            $interval.cancel(stop);
            stop = undefined;
            $scope.model.monitoringDatasets = false;
        }
    }

    $scope.$on('$destroy', function () {
        // Make sure that the interval is destroyed too
        $scope.stopFight();
    });

    $scope.processDataset = function (dsId) {
        $scope.model.message = "get dataset id: " + dsId;
    }

   ///////////////////////////////////////////
    //Paging
    /*
    var setPaging = function (currentPage) {
        $scope.pagingSettings = {
            currentPage: currentPage,
            offset: 0,
            pageLimit: MAX_ROWS,
            pageLimits: ['5', '10', '50', '100']
        };
    }
    setPaging(0);
    */

    $scope.model.message = "Controller initialized";
}