/// <reference path="../asset/admin/libs/angular.min.js" />


(function () {
    angular.module("nihility", ['nihility.products', 'nihility.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('home', {
            url: "/admin",
            templateUrl: "/app/components/Home/HomeView.html",
            controller: "HomeController"
        });

        $urlRouterProvider.otherwise('/admin');
    }
});