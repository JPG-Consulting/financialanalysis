function edgarServ($http) {
    
    var URL_SERVICE = "edgar_api/datasets/all"

    ////////////////////////////////
    //Private
    var getPromise = function () {
        var promise = $http({
            method: "GET",
            url: URL_SERVICE,
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

        var url = '/edgar_api/datasets/process';
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

        $http.post(url, '"' + id + '"').success(callbackSuccess).error(callbackError);
    };


}