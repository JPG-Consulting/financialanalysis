///////////////////////////////////////////////////////////////////////////////////
//Controller

function indexCtrl($scope,$interval, serv) {
    $scope.model = new Object();

    $scope.model.Respuesta = "Backend EDGAR";
    
    var stop;
    $scope.startMonitorinDatasets_click = function () {
        $scope.model.message = "mostrar datasets";
        
        // Don't start a new fight if we are already fighting
        if (angular.isDefined(stop))
            return;
        $scope.model.monitoringDatasets = true;
        stop = $interval(function () {
            serv.getDatasets(
                //sucess callback
                function (rawDatasets) {
                    $scope.model.datasets = rawDatasets;
                    $scope.model.datasetsMessage = "Executing at: " + (new Date());
                },
                //error callback
                function (response) {
                    $scope.model.errorMessage = response;
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

    $scope.showDatasets_click = function()
    {
        serv.getDatasets(
                //sucess callback
                function (rawDatasets) {
                    $scope.model.datasets = rawDatasets;
                    $scope.model.datasetsMessage = "Executing at: " + (new Date());
                },
                //error callback
                function (response) {
                    $scope.model.errorMessage = response;
                }
            );
    }

    $scope.processDataset = function (dsId) {
        $scope.model.message = "get dataset id: " + dsId;
        serv.processDataset(dsId, processDatasetCallbackSuccess, processDatasetCallbackError);
    }

    var processDatasetCallbackSuccess = function (data, status, headers, config) {
        //alert("processDataset_sucesscallback: " + data);
        //$scope.model.message = data;
        $scope.model.message = "Process started";
        $scope.model.datasets = data;
    }

    var processDatasetCallbackError = function (data, status, header, config) {
        $scope.model.errorMessage =
            "Message: " + data.data.Message + "<br>" +
            "Message detail: " + data.data.MessageDetail + "<br>" +
            "status: " + status + "<br>" +
            "headers: " + header + "<br>" +
            "config: " + config;
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