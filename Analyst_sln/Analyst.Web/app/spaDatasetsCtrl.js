///////////////////////////////////////////////////////////////////////////////////
//Controller

function spaDatasetsCtrl($scope, $interval, serv) {
    $scope.model = new Object();

    $scope.model.Title = "EDGAR datasets status";
    
    //////////////////////////////////////////////////////////////////////////////////////////////////
    //Timer
    var stop;
    var seconds = 1;
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
                    $scope.message = "Error startMonitorinDatasets_click";
                    $scope.model.errorMessage = response.data;
                }
            );
            
        }, seconds * 1000);
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

    //////////////////////////////////////////////////////////////////////////////////////////////////
    //Show datasets
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
                    $scope.model.message = "Error in showDatasets_click";
                    $scope.model.errorMessage = response.data;
                }
            );
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////
    //Process datasets
    $scope.processDataset_click = function (dsId) {
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
        if (data.data != undefined) {
            $scope.model.errorMessage =
                "Message: " + data.data.Message + "<br>" +
                "Message detail: " + data.data.MessageDetail + "<br>" +
                "status: " + status + "<br>" +
                "headers: " + header + "<br>" +
                "config: " + config;
        }
        else
        {
            $scope.model.errorMessage = data;
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////

    
    $scope.progressNumberStyle = function (value) {
        var iValue = parseInt(value);
        if (iValue == 100)
            return { color: 'green' };
        else if (0.0001 < iValue && iValue < 100)
            return { color: 'blue' };
        else if (iValue == 0 || isNaN(iValue))
            return { color: 'grey' };
        else
            return { color: 'red' };
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////
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