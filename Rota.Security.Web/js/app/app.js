'use strict';

angular.module("app", ["ngRoute"]).config(function ($routeProvider) {
    //Add routes
    $routeProvider.when('/login', {
        templateUrl: '/js/app/partials/login.html',
        controller: 'loginController'
    }).when('/home', {
        templateUrl: '/js/app/partials/home.html',
        controller: 'homeController'
    }).when('/profile', {
        templateUrl: '/js/app/partials/profile.html',
        controller: 'profileController'
    }).otherwise({ redirectTo: '/' });

}).value('currentUser', {})
  .factory('authorization', ['$rootScope', '$http', '$location', 'currentUser',
    function ($rootScope, $http, $location, currentUser) {
        return {
            initIdentity: initIdentity,
            getUserInfo: getUserInfo,
            login: login,
            logout: logout
        };
        //
        function logout() {

        }
        //
        function login(username, password, rememberme) {
            //
            var xsrf = $.param({
                grant_type: 'password',
                username: username,
                password: password
            });

            return $http({
                method: 'POST',
                url: '/Token',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                data: xsrf
            }).then(function (result) {
                //
                angular.extend(currentUser || {}, result.data);
                //
                if (result.data.userName && result.data.access_token) {
                    //
                    initIdentity(result.data.access_token);
                    //
                    if (rememberme) {
                        localStorage.setItem('accessToken', result.data.access_token);
                    } else {
                        sessionStorage.setItem('accessToken', result.data.access_token);
                    }
                    //
                    $location.path('/home');
                }
            }, function (err) {
                debugger;
            });
        }
        //
        function getUserInfo() {
            var self = this;
            return $http.get('/api/Account/UserInfo').then(function (result) {
                currentUser.userName = result.data.userName;
            }, function (err) {
                throw err;
            });
        }
        //Check authorization
        function initIdentity(token) {
            var hash = token || sessionStorage["accessToken"] || localStorage["accessToken"];
            if (hash !== undefined && hash) {
                $http.defaults.headers.common.Authorization = 'Bearer ' + hash;
            }
            return hash;
        }
    }]).controller('loginController', ['$scope', 'authorization',
    function ($scope, authorization) {
        $scope.login = function () {
            authorization.login($scope.username, $scope.password, $scope.rememberme);
        };
        $scope.logout = function () {
            authorization.logout();
        };
    }]).controller('homeController', function (authorization) {

    }).controller('profileController', ['currentUser', function (currentUser) {
        if (currentUser.userData === undefined) {
            $location.path('login');
        }
    }]).run(['$location', 'authorization', function ($location, authorization) {
        //
        authorization.initIdentity();
        //
        authorization.getUserInfo().then(function () {
            $location.path('/home');
        }, function (err) {
            $location.path('/login');
        });
    }]);