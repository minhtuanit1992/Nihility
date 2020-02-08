/// <reference path="../../../asset/admin/libs/angular.min.js" />

(function () {
    angular.module('nihility.products', ['nihility.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('Home', {
            url: "/products",
            templateUrl: "/app/Components/Products/ProductListView.html",
            controller: "ProductListController"
        }).state('product_add', {
            url: "/product_add",
            templateUrl: "/app/Components/Products/ProductAddView.html",
            controller: "ProductAddController"
        });
    }
})();