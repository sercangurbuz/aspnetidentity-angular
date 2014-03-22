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
    }).when('/register', {
        templateUrl: '/js/app/partials/register.html',
        controller: 'registerController'
    }).when('/admin/users', {
        templateUrl: '/js/app/partials/users.html',
        controller: 'usersController'
    }).when('/admin/user/:id', {
        templateUrl: '/js/app/partials/user.html',
        controller: 'userController'
    }).otherwise({ redirectTo: '/' });

}).value('currentUser', {}).factory('userManager', ['$http', function ($http) {
    return {
        getUsers: getUsers,
        getRoles: getRoles,
        getUserById: getUserById
    };

    function getUsers() {
        return $http.get('/api/Account/GetUsers');
    }

    function getRoles() {
        return $http.get('/api/Account/GetRoles');
    }

    function getUserById(id) {
        return $http.get('/api/Account/GetUserById/' + id);
    }
}]).factory('authorization', ['$rootScope', '$http', '$location', 'currentUser',
    function ($rootScope, $http, $location, currentUser) {
        return {
            initIdentity: initIdentity,
            getUserInfo: getUserInfo,
            login: login,
            logoff: logoff,
            register: register
        };
        //Logoff method
        function logoff() {
            $http.post('/api/Account/Logout').then(function () {
                //Token bilgileri sil
                localStorage.removeItem("accessToken");
                sessionStorage.removeItem("accessToken");
                //Go login
                $location.path('/login');
            });
        }
        //Login method
        function login(username, password, rememberme) {
            //TODO:Param alternatif araştirilmali
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
                //Kullancii nesnesi guncelleniyor
                currentUser = angular.extend(currentUser || {}, result.data);
                //Tokeni persistant bi yerlere at
                if (result.data.userName && result.data.access_token) {
                    //Http header'i token bearer ile decorate et
                    initIdentity(result.data.access_token);
                    //Remember me ?
                    if (rememberme) {
                        localStorage.setItem('accessToken', result.data.access_token);
                    } else {
                        sessionStorage.setItem('accessToken', result.data.access_token);
                    }
                    //Go home yanki !
                    $location.path('/home');
                }
            }, function (err) {
                throw err;
            });
        }
        //Kullanci bilgileri al tokene gore
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
        //Register user
        function register(userName, password, confirmPassword) {
            return $http.post('/api/Account/Register', {
                userName: userName,
                password: password,
                confirmPassword: confirmPassword
            });
        }
    }]).controller('loginController', ['$scope', 'authorization',
    function ($scope, authorization) {
        $scope.login = function () {
            $scope.errors = [];
            authorization.login($scope.username, $scope.password, $scope.rememberme).catch(function (errs) {
                $scope.errors.push(errs.data.error_description);
            });
        };
        $scope.logout = function () {
            authorization.logout();
        };
    }]).controller('shellController', ['$scope', 'currentUser', 'authorization', function ($scope, currentUser, authorization) {
        $scope.currentUser = currentUser;
        $scope.logoff = function () {
            authorization.logoff();
        };
    }]).controller('homeController', function (authorization) {

    }).controller('profileController', ['currentUser', function (currentUser) {
        if (currentUser.userData === undefined) {
            $location.path('login');
        }
    }]).controller('usersController', ['$scope', 'userManager', function ($scope, userManager) {
        userManager.getUsers().then(function (result) {
            $scope.users = result.data;
        });
    }]).controller('userController', ['$scope', '$routeParams', 'userManager', function ($scope, $routeParams, userManager) {

        debugger;
        userManager.getRoles().then(function (result) {
            $scope.roles = result.data;
        });

        var id = parseInt($routeParams.id);

        if (id > 0) {
            userManager.getUserById(id).then(function (result) {
                $scope.user = result.data;
            });
        }

        $scope.removeRole = function (role) {

        };
    }]).controller('registerController', ['$scope', 'authorization', function ($scope, authorization) {
        $scope.register = function () {
            $scope.errors = [];
            authorization.register($scope.userName, $scope.password, $scope.confirmPassword).then(function (result) {
                authorization.login($scope.userName, $scope.password);
            }, function (errs) {
                var errors = [];
                if (errs.data.modelState) {
                    for (var err in errs.data.modelState) {
                        var errorItems = errs.data.modelState[err];
                        if (errorItems.length > 0) {
                            angular.forEach(errorItems, function (item) {
                                errors.push(item);
                            });
                        }
                    }
                }
                $scope.errors = $scope.errors.concat(errors.length > 0 ? errors : [errs.data.message || 'Unknown error !']);
            });
        };
    }])
    .run(['$location', 'authorization', function ($location, authorization) {
        //Eger persistant bir token varsa http requesti decore ediyoruz
        authorization.initIdentity();
        //Aktif kullanici bilgisini al
        authorization.getUserInfo().then(function () {
            //Ana sayfaya git
            $location.path('/home');
        }, function (err) {
            //Proceed to login !
            $location.path('/login');
        });
    }]);