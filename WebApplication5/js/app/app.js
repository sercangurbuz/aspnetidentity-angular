'use strict';

angular.module("app", ["ngRoute"]).config(function ($routeProvider) {
    //Add routes
    $routeProvider.when('login', {
        templateUrl: '/app/partials/login.html',
        controller: 'loginController'
    }).when('home', {
        templateUrl: '/app/partials/home.html',
        controller: 'homeController'
    }).when('profile', {
        templateUrl: '/app/partials/profile.html',
        controller: 'profileController'
    });

}).constant('urls',
{
    tokenUrl: '/token',
    userInfoUrl: '/api/Account/UserInfo'
}
).factory('authorization', ['$rootScope', '$http', '$location', 'urls', function ($rootScope, $http, $location, urls) {
    var userData;
    return {
        checkIdentity: checkIdentity,
        setUserInfo: setUserInfo,
        getUserInfo: getUserInfo
    };
    //
    function getUserInfo() {
        return this.userData;
    }
    //
    function setUserInfo(userData) {
        this.userData = userData;
        $http.defaults.headers.common.Authorization = 'Bearer ' + encoded;
    }
    //Check authorization
    function checkIdentity() {
        var token = sessionStorage["accessToken"] || localStorage["accessToken"];

        $http.get(urls.tokenUrl, {}).then(function (data) {
            debugger;
            $rootScope.userName = data.userName;
        }, function (err) {
            $location.path('login');
        });
    }
}]).controller('loginController', ['authorization', function (authorization) {

}]).controller('homeController', function (authorization) {

}).controller('profileController', ['authorization', function (authorization) {

}]).run(['authorization', function (authorization) {
    //
    authorization.checkIdentity();

}]);