///////////////////////////////////////////////////////////////////////////////////
//Controller

function indexCtrl($scope, serv) {
    $scope.model = new Object();

    $scope.model.Respuesta = "Backend EDGAR";
    
    $scope.datasets_click = function(){
        $scope.model.message = "mostrar datasets";
        serv.getDatasets(
            function (datasets) {
                $scope.model.datasets = datasets;
            }
        );
    }

    $scope.selectDataset = function (dsId) {
        alert("get dataset id: " + dsid);
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