/// <reference path="../plugins/angular.min.js" />

/// <summary>
/// - Khai báo một Module trong AngularJS
/// </summary>
/// <param name="moduleName">Tên Module</param>
/// <param name="Dependency">Danh sách các thành phần phụ thuộc</param>
let myApp = angular.module("myModule", []);

/// <summary>
/// - Khai báo một Controller trong AngularJS
/// </summary>
/// <param name="controllerName">Tên Controller</param>
/// <param name="func">Phương thức sẽ thực hiện trên Controller</param>
myApp.controller("myController", myController);
myApp.service("ValidatorService", ValidatorService);
//myApp.directive("NihiDirective", NihiDirective);

myController.$inject = ['$scope', 'ValidatorService'];

function myController($scope, ValidatorService) {
    $scope.message = "This is my message from Controller";
    ValidatorService.checkNumber(1);
}

/// <summary>
/// - Khai báo một Service trong AngularJS
/// </summary>
/// <param name="serviceItem"></param>
function ValidatorService($window) {
    return {
        checkNumber: checkNumber
    };
    function checkNumber(input) {
        if (input % 2 === 0) {
            $window.alert('This is even');
        } else {
            $window.alert('This is odd');
        }
    }
}

function NihiDirective() {
    return {
        template: "<h1>Hello World</h1>"
    };
}