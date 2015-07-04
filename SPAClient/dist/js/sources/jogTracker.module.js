angular.module('jogTracker', [
    'jogTracker.api',
    'jogTracker.validation',
    'ui.router',
    'ui.bootstrap',
    'ngAnimate',
    'toastr', //"toaster" for notification messages
    'angular-md5' //Needed to hash user email for gravatar
    ])

    .config(function($stateProvider, $urlRouterProvider){

        //By default, go home.
        $urlRouterProvider.otherwise("/");

        $stateProvider
            .state('login', {
                url: "/login",
                templateUrl: "partials/login.html"
            })
            .state('register', {
                url: "/register",
                templateUrl: "partials/register.html"
            })
            .state('main', {
                url: "/",
                templateUrl: "partials/main.html"
            })
            .state('main.jogs', {
                url: "jogs",
                templateUrl: "partials/main.jogs.html"
            })
            .state('main.reports', {
                url: "reports",
                templateUrl: "partials/main.reports.html"
            })
            .state('main.admin', { //For admin functions, like user maintenance.
                url: "admin",
                templateUrl: "partials/main.admin.html"
            })
            .state('main.account', {
                url: "account",
                templateUrl: "partials/main.account.html"
            });
    });
