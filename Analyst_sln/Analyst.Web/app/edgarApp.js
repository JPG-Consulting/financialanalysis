'use strict';

//var edgarApp = angular.module('edgarApp', ['angularSimplePagination']);
var edgarApp = angular.module('edgarApp', []);

edgarApp.service('edgarServ', ['$http', edgarServ]);

edgarApp.controller('spaEdgarDatasetsCtrl', spaEdgarDatasetsCtrl);
spaEdgarDatasetsCtrl.$inject = ['$scope', '$interval', 'edgarServ'];


edgarApp.controller('spaEdgarCtrl', spaEdgarCtrl);
spaEdgarCtrl.$inject = ['$scope', '$interval', 'edgarServ'];
