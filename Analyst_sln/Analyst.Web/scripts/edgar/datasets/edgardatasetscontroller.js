///////////////////////////////////////////////////////////////////////////////////
//Controller

function edgardatasetscontroller($scope, $interval, serv) {
    $scope.model = new Object();

    $scope.model.Title = "EDGAR datasets status";
    
    //////////////////////////////////////////////////////////////////////////////////////////////////
    //Timer
    var stop;
    var seconds = 5;
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
        $scope.model.message = "processing dataset id: " + dsId;
        serv.processDataset(dsId, processDatasetCallbackSuccess, callbackError);
    }

    $scope.deleteDataset_click = function (file, dsId) {
        if (confirm("Desea eliminar el archivo '" + file + "' del dataset " + dsId)) {
            serv.deleteDataset(dsId, file, deleteDatasetCallbackSuccess, callbackError);
        }
    }

    var deleteDatasetCallbackSuccess = function (data, status, headers, config) {
        alert("Dataset deleted");
        $scope.model.datasets = data;
    }

    var processDatasetCallbackSuccess = function (data, status, headers, config) {
        alert("Process started");
        $scope.model.datasets = data;
    }

    var callbackError = function (data, status, header, config) {
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

    $scope.test_click = function (dsId) {
        //chkContinueIfErrors
        //radioWithNotes
        //radioWithoutNotes
        $scope.model.message = "valor chks: chkContinueIfErrors=";
        
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

    $scope.model.message = "Edgar Datasets Controller initialized";
}