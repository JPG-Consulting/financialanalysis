function edgarServ($http) {
    
    var URL_GET_ALL_DATASETS = "api/all"
    var URL_PROCESS_DATASET = "api/process";
    var URL_GET_REGISTRANTS = "askedgarapi/companies"
    ////////////////////////////////
    //Private
    var getPromise = function (pUrl) {
        var promise = $http({
            method: "GET",
            url: pUrl,
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
        getPromise(URL_GET_ALL_DATASETS).then
        (
            function success(response) {
                successCallback(response.data);
            },
            function error(response) {
                errorCallback(response);
            }
        );
    };

    this.getRegistrants = function (successCallback, errorCallback) {
        getPromise(URL_GET_REGISTRANTS).then
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
        //post example:
        //http://localhost:1326/edgardatasetsapi/datasets/process?id=201901

        $http.post(URL_PROCESS_DATASET, '"' + id + '"').success(callbackSuccess).error(callbackError);
    };


}