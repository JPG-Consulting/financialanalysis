///////////////////////////////////////////////////////////////////////////////////
//Controller

function spaEdgarCtrl($scope, $interval, serv) {
    $scope.model = new Object();

    $scope.model.Title = "Ask Edgar";

    $scope.showDatasets_click = function () {
        serv.getDatasets(
                //sucess callback
                function (rawDatasets) {
                    $scope.model.resultData = rawDatasets;
                    $scope.model.statusMessage = "Executing at: " + (new Date());
                },
                //error callback
                function (response) {
                    $scope.model.message = "Error in showDatasets_click";
                    $scope.model.errorMessage = response.data;
                }
            );
    }
}
