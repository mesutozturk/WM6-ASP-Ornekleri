/// <reference path="angular.js" />

var app = angular.module("myApp", []);

app.controller("ProductCtrl", function ($scope) {
    $scope.urunler = [];

    function init() {
        var data = JSON.parse(localStorage.getItem("urunler"));
        $scope.urunler = data === null ? [] : data;
    }
    $scope.ekle = function () {
        $scope.urunler.push({
            id: guid(),
            urunAdi: $scope.yeni.urunAdi,
            fiyat: $scope.yeni.fiyat,
            eklenmeZamani: new Date()
        });
        $scope.yeni.urunAdi = "";
        $scope.yeni.fiyat = "";
        localStorage.setItem("urunler", JSON.stringify($scope.urunler));
    };

    function guid() {
        function S4() {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        }

        // then to call it, plus stitch in '4' in the third group
        return (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
    }

    init();
});