function edgarServ($http) {
    
    var URL_GET_ALL = "edgar_api/datasets/all"
    var URL_PROCESS = "edgar_api/datasets/process";
    ////////////////////////////////
    //Private
    var getPromise = function () {
        var promise = $http({
            method: "GET",
            url: URL_GET_ALL,
            cache: false
        });
        promise.success(
            function (data, status) {
                return data;
            }
       );

        promise.error(
            function (data, status) {
                console.log(data, status);
                return { "status": false };
            }
        );
        return promise;
    };


    ////////////////////////////////
    //Public
    this.getDatasets = function (successCallback, errorCallback) {
        getPromise().then
        (
            function success(response) {
                successCallback(response.data);
            },
            function error(response) {
                errorCallback(response);
            }
        );

    };

    this.processDataset = function (id, callbackSuccess, callbackError) {
        //post:
        //http://localhost:1326/edgar_api/datasets/process?id=201901

        
        /*
        var req = {
            method: 'POST',
            url: '/edgar_api/datasets/process',
            headers: {
                //'Content-Type': undefined
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            },
            data: { dsId: id }
        }
        $http(req).then(callbackSuccess, callbackError);
        */

        $http.post(URL_PROCESS, '"' + id + '"').success(callbackSuccess).error(callbackError);
    };


}