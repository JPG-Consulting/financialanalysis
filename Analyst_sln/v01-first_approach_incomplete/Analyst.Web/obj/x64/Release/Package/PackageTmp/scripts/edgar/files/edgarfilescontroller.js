///////////////////////////////////////////////////////////////////////////////////
//Controller

function edgarfilescontroller($scope, $interval, serv) {
    $scope.model = new Object();

    $scope.model.Title = "EDGAR files status";


    //////////////////////////////////////////////////////////////////////////////////////////////////
    //Timer
    var stop;
    var seconds = 5;
    $scope.startMonitoringIndexes_click = function () {
        $scope.model.message = "mostrar listado de master.idx";

        // Don't start a new fight if we are already fighting
        if (angular.isDefined(stop))
            return;
        $scope.model.monitoringIndexes = true;
        stop = $interval(function () {
            serv.getFullIndexes(
                //sucess callback
                function (rawIndexes) {
                    $scope.model.indexes = rawIndexes;
                    $scope.model.indexesMessage = "Executing at: " + (new Date());
                },
                //error callback
                function (response) {
                    $scope.message = "Error startMonitoringIndexes_click";
                    $scope.model.errorMessage = response.data;
                }
            );

        }, seconds * 1000);
    }

    $scope.stopMonitorinIndexes_click = function () {
        if (angular.isDefined(stop)) {
            $interval.cancel(stop);
            stop = undefined;
            $scope.model.monitoringIndexes = false;
        }
    }

    $scope.$on('$destroy', function () {
        // Make sure that the interval is destroyed too
        $scope.stopFight();
    });

    //////////////////////////////////////////////////////////////////////////////////////////////////
    //Show datasets
    $scope.showFullIndexes_click = function () {
        serv.getFullIndexes(
            //sucess callback
            function (rawIndexes) {
                $scope.model.indexes = rawIndexes;
                $scope.model.indexesMessage = "Executing at: " + (new Date());
            },
            //error callback
            function (response) {
                $scope.model.message = "Error in showIndexes_click";
                $scope.model.errorMessage = response.data;
            }
        );
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////
    //Process indexes
    $scope.processFullIndex_click = function (year, quarter) {
        $scope.model.message = "get index year: " + year + ", quarter: " + quarter;
        serv.processFullIndex(year, quarter, processIndexCallbackSuccess, processIndexCallbackError);
    }

    var processIndexCallbackSuccess = function (data, status, headers, config) {
        $scope.model.message = "Process started";
        $scope.model.indexes = data;
    }

    var processIndexCallbackError = function (data, status, header, config) {
        if (data.data != undefined) {
            $scope.model.errorMessage =
                "Message: " + data.data.Message + "<br>" +
                "Message detail: " + data.data.MessageDetail + "<br>" +
                "status: " + status + "<br>" +
                "headers: " + header + "<br>" +
                "config: " + config;
        }
        else {
            $scope.model.errorMessage = data;
        }
    }
    //////////////////////////////////////////////////////////////////////////////////////////////////
    //auxiliary functions

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

    $scope.model.message = "Edgar Files Controller initialized";
}