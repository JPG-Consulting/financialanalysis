'use strict';

//var edgarApp = angular.module('edgarApp', ['angularSimplePagination']);
var edgarApp = angular.module('edgarApp', []);

edgarApp.service('edgarServ', ['$http', edgarServ]);

edgarApp.controller('indexCtrl', indexCtrl);
indexCtrl.$inject = ['$scope','$interval', 'edgarServ'];



