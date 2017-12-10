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
    this.getDatasets = function (successCallback) {
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

    

}