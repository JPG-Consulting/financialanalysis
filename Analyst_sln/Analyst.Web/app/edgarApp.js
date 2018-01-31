'use strict';

//var edgarApp = angular.module('edgarApp', ['angularSimplePagination']);
var edgarApp = angular.module('edgarApp', []);

edgarApp.service('edgarServ', ['$http', edgarServ]);

edgarApp.controller('spaDatasetsCtrl', spaDatasetsCtrl);
spaDatasetsCtrl.$inject = ['$scope', '$interval', 'edgarServ'];



