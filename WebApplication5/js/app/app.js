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
        function login(username, password) {
            return $http.post('/Token', {
                grant_type: "password",
                username: username,
                password: password
            }).then(function (data) {
                debugger;
            }, function (err) {
                debugger;
            });
        }
        //
        function getUserInfo() {
            var self = this;
            return $http.get('/api/Account/UserInfo').then(function (data) {
                debugger;
                currentUser.userData = data;
            }, function (err) {
                throw err;
            });
        }
        //Check authorization
        function initIdentity(token) {
            var hash = token || sessionStorage["accessToken"] || localStorage["accessToken"];
            if (hash !== undefined && !hash) {
                $http.defaults.headers.common.Authorization = 'Bearer ' + hash;
            }
            return hash;
        }
    }]).controller('loginController', ['$scope', 'authorization',
        function ($scope, authorization) {
            return {
                login: function () {
                    authorization.login($scope.username, $scope.password);
                },
                logout: function () {
                    authorization.logout();
                }
            };
        }]).controller('homeController', function (authorization) {

        }).controller('profileController', ['currentUser', function (currentUser) {
            if (currentUser.userData === undefined) {
                $location.path('login');
            }
        }]).run(['$location','authorization', function ($location,authorization) {
            //
            debugger;
            authorization.initIdentity();
            //
            authorization.getUserInfo().catch(function (err) {
                debugger;
                $location.path('login');
            });
        }]);