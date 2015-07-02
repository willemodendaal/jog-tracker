(function(){
    var authModule = angular.module('jogTracker.api.auth', [])
        .config(function($httpProvider) {
            $httpProvider.interceptors.push('loginInterceptor');
        });

    authModule.value('userInfo', { auth_token: null }); //Will be updated by the loginInterceptor.
}());
