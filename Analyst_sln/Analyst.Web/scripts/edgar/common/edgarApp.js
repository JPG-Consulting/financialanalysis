'use strict';

//var edgarApp = angular.module('edgarApp', ['angularSimplePagination']);
var edgarApp = angular.module('edgarApp', []);

edgarApp.service('edgarServ', ['$http', edgarServ]);

edgarApp.controller('edgardatasetscontroller', edgardatasetscontroller);
edgardatasetscontroller.$inject = ['$scope', '$interval', 'edgarServ'];

edgarApp.controller('askedgarcontroller', askedgarcontroller);
askedgarcontroller.$inject = ['$scope', '$interval', 'edgarServ'];
