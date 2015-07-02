(function(){
    angular.module('jogTracker.api.auth', ['$httpProvider'])
        .config(function($httpProvider) {
            $httpProvider.interceptors.push('APIInterceptor');
        });
}());
