///////////////////////////////////////////////////////////////////////////////////
//Controller


function askedgarcontroller($scope, $interval, serv) {
    $scope.model = new Object();

    $scope.model.Title = "Ask Edgar";
    
    $scope.showCompanies_click = function () {
        serv.getRegistrants(
                //sucess callback
                function (rawData) {
                    $scope.model.companies = rawData;
                    $scope.model.statusMessage = "Executing at: " + (new Date());
                },
                //error callback
                function (response) {
                    $scope.model.message = "Error in showCompanies_click";
                    $scope.model.errorMessage = response.data;
                }
            );
    }

}
