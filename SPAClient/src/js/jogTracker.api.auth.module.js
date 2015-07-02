(function(){
    var authModule = angular.module('jogTracker.api.auth', ['$httpProvider'])
        .config(function($httpProvider) {
            $httpProvider.interceptors.push('loginInterceptor');
        });

    authModule.value('authToken', null); //Will be updated by the loginInterceptor.
}());
